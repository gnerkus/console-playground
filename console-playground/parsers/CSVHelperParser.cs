using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using CsvHelper;
using CsvHelper.Configuration;

namespace Main.parsers
{
    public class Foo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Empty;
    
    public static class CSVHelperParser
    {
        private static TypeBuilder BuildDynamicType()
        {
            var parent = typeof(Empty);
            const string name = "CSVRow";
            
            // 1. create assembly name
            var assemblyName = new AssemblyName($"SomeAssemblyName{name}");
            // 2. create the assembly builder
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            // 3. use assembly builder to create a module builder
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            // 4. use module builder to create a type builder
            var tb = moduleBuilder.DefineType(name,
                TypeAttributes.Public |
                TypeAttributes.Class
                , parent);
            return tb;
        }

        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            var fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            var propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            var getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            var getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            var setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                    MethodAttributes.Public |
                    MethodAttributes.SpecialName |
                    MethodAttributes.HideBySig,
                    null, new[] { propertyType });

            var setIl = setPropMthdBldr.GetILGenerator();
            var modifyProperty = setIl.DefineLabel();
            var exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
        public static IEnumerable<Foo> SimpleParse(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            };
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<Foo>();
                return records.ToList();
            }
        }

        public static IEnumerable<object> DynamicParse(string filePath)
        {
            var dynamicTypeBuilder = BuildDynamicType();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            };
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            csv.Read();
            csv.ReadHeader();
            
            // add properties to dynamic type based on csv.HeaderRecord
            foreach (var prop in csv.HeaderRecord)
            {   
                CreateProperty(dynamicTypeBuilder, prop, typeof(string));
            }

            var dynamicType = dynamicTypeBuilder.CreateType();
            
            // mapping
            //csv.Context.RegisterClassMap(new CSVRowMap(csv.HeaderRecord));

            var records = new List<object>();
            while (csv.Read())
            {
                // Get the generic type definition
                // var method = typeof(CsvReader).GetMethod("GetRecord", 
                //     BindingFlags.Public | BindingFlags.Instance);
                //
                // Console.WriteLine("METHOD");
                // Console.WriteLine(method);
                //
                // // Build a method with the specific type argument you're interested in
                // method = method.MakeGenericMethod(dynamicType);
                // var csvRecord = method.Invoke(csv, []);

                var csvRecord = csv.GetRecord(dynamicType);
                
                records.Add(csvRecord);
            }
            
            return records;
        }
    }
}