namespace Morpeh.TestSuite.Editor {
    using System;

    [Serializable]
    internal struct TestComponent : IComponent, IEquatable<TestComponent> {
        public int   data0;
        public float data1;
        public bool  data2;

        public bool Equals(TestComponent other) => this.data0 == other.data0 && this.data1.Equals(other.data1) && this.data2 == other.data2;

        public override bool Equals(object obj) => obj is TestComponent other && Equals(other);

        public override int GetHashCode() {
            unchecked {
                var hashCode = this.data0;
                hashCode = (hashCode * 397) ^ this.data1.GetHashCode();
                hashCode = (hashCode * 397) ^ this.data2.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(TestComponent left, TestComponent right) => left.Equals(right);

        public static bool operator !=(TestComponent left, TestComponent right) => !left.Equals(right);
    }
        
    [Serializable]
    internal struct TestMarker : IComponent {  }
}