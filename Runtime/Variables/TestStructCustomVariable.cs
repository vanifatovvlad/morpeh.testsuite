namespace Morpeh.Unity.Tests.Runtime.Variables {
    using System;
    using Globals;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Globals/Custom/Tests/" + nameof(TestStructCustomVariable))]
    public class TestStructCustomVariable : BaseGlobalVariable<DummyStruct> {
        public override DataWrapper Wrapper { get; set; }

        public override DummyStruct Deserialize(string serializedData) => JsonUtility.FromJson<DummyStruct>(serializedData);
        
        public override string Serialize(DummyStruct data) => JsonUtility.ToJson(data);
    }

    [Serializable]
    public struct DummyStruct {
        public int clang;
        public int cpp;
        public int csharp;
    }
}