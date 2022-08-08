#if MORPEH_GLOBALS

namespace Morpeh.Unity.Tests.Runtime.Variables {
    using System;
    using Globals;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Globals/Custom/Tests/" + nameof(TestClassCustomVariable))]
    public class TestClassCustomVariable : BaseGlobalVariable<DummyClass> {
        public override DataWrapper Wrapper { get; set; }

        public override DummyClass Deserialize(string serializedData) {
            if (!string.IsNullOrEmpty(serializedData)) {
                if (this.value != null) {
                    JsonUtility.FromJsonOverwrite(serializedData, this.value);
                }
                else {
                    this.value = JsonUtility.FromJson<DummyClass>(serializedData);
                }
            }
            return this.value;
        }
        public override string Serialize(DummyClass data) => JsonUtility.ToJson(this.value);
    }

    [Serializable]
    public class DummyClass {
        public int clang;
        public int cpp;
        public int csharp;
    }
}

#endif