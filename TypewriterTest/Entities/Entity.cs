using System;

namespace TypewriterTest.Entities {
    [TypeScript]
    public class Entity {
        public Guid Id { get; set; }
    }

    public class TypeScriptAttribute : Attribute { }

    public class TypeScriptIgnoreAttribute : Attribute { }
}