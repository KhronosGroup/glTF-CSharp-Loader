using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class AccessorSparseIndices
    {

        /// <summary>
        /// Backing field for BufferView.
        /// </summary>
        private int m_bufferView;

        /// <summary>
        /// Backing field for ByteOffset.
        /// </summary>
        private int m_byteOffset = 0;

        /// <summary>
        /// Backing field for ComponentType.
        /// </summary>
        private ComponentTypeEnum m_componentType;

        /// <summary>
        /// Backing field for Extensions.
        /// </summary>
        private System.Collections.Generic.Dictionary<string, object> m_extensions;

        /// <summary>
        /// Backing field for Extras.
        /// </summary>
        private Extra m_extras;

        public int BufferView
        {
            get
            {
                return this.m_bufferView;
            }
            set
            {
                if ((value < 0))
                {
                    throw new System.ArgumentOutOfRangeException("BufferView", value, "Expected value to be greater than or equal to 0");
                }
                this.m_bufferView = value;
            }
        }

        public int ByteOffset
        {
            get
            {
                return this.m_byteOffset;
            }
            set
            {
                if ((value < 0))
                {
                    throw new System.ArgumentOutOfRangeException("ByteOffset", value, "Expected value to be greater than or equal to 0");
                }
                this.m_byteOffset = value;
            }
        }

        public ComponentTypeEnum ComponentType
        {
            get
            {
                return this.m_componentType;
            }
            set
            {
                this.m_componentType = value;
            }
        }

        public System.Collections.Generic.Dictionary<string, object> Extensions
        {
            get
            {
                return this.m_extensions;
            }
            set
            {
                this.m_extensions = value;
            }
        }

        public Extra Extras
        {
            get
            {
                return this.m_extras;
            }
            set
            {
                this.m_extras = value;
            }
        }

        public bool IsLocked { get => isLocked; set => isLocked = value; }

        public bool ShouldSerializeByteOffset()
        {
            return ((m_byteOffset == 0)
                        == false);
        }

        public bool ShouldSerializeExtensions()
        {
            return ((m_extensions == null)
                        == false);
        }

        public bool ShouldSerializeExtras()
        {
            return ((m_extras == null)
                        == false);
        }

        public enum ComponentTypeEnum
        {

            UNSIGNED_BYTE = 5121,

            UNSIGNED_SHORT = 5123,

            UNSIGNED_INT = 5125,
        }

        private bool isLocked = false;
        public void Lock()
        {
            isLocked = true;
        }
        public void Unlock()
        {
            isLocked = false;
        }

    }
}
