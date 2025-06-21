using System;
using System.IO;

namespace ER.Metadata.Decryptor
{
    class Program
    {
        private static String m_MetaFile = "global-metadata.dat";
        private static String m_MetaFileOut = "global-metadata-dec.dat";

        static void Main(String[] args)
        {
            if (!File.Exists(m_MetaFile))
            {
                throw new FileNotFoundException("[ERROR]: Unable to open " + m_MetaFile + " file!");
            }

            var lpBuffer = File.ReadAllBytes(m_MetaFile);
            using (MemoryStream TMetaStream = new MemoryStream(lpBuffer))
            {
                var m_Header = new Il2CppGlobalMetadataHeader();

                m_Header.magic = TMetaStream.ReadInt32();

                if (m_Header.magic != 0x504754)
                {
                    throw new Exception("[ERROR]: Invalid magic of metadata file!");
                }

                m_Header.sanity = TMetaStream.ReadInt32();
                m_Header.version = TMetaStream.ReadInt32();
                m_Header.stringLiteralOffset = TMetaStream.ReadInt32();
                m_Header.stringLiteralCount = TMetaStream.ReadInt32();
                m_Header.stringLiteralDataOffset = TMetaStream.ReadInt32();
                m_Header.stringLiteralDataCount = TMetaStream.ReadInt32();
                m_Header.stringOffset = TMetaStream.ReadInt32();
                m_Header.stringCount = TMetaStream.ReadInt32();
                m_Header.eventsOffset = TMetaStream.ReadInt32();
                m_Header.eventsCount = TMetaStream.ReadInt32();
                m_Header.propertiesOffset = TMetaStream.ReadInt32();
                m_Header.propertiesCount = TMetaStream.ReadInt32();
                m_Header.methodsOffset = TMetaStream.ReadInt32();
                m_Header.methodsCount = TMetaStream.ReadInt32();
                m_Header.parameterDefaultValuesOffset = TMetaStream.ReadInt32();
                m_Header.parameterDefaultValuesCount = TMetaStream.ReadInt32();
                m_Header.fieldDefaultValuesOffset = TMetaStream.ReadInt32();
                m_Header.fieldDefaultValuesCount = TMetaStream.ReadInt32();
                m_Header.fieldAndParameterDefaultValueDataOffset = TMetaStream.ReadInt32();
                m_Header.fieldAndParameterDefaultValueDataCount = TMetaStream.ReadInt32();
                m_Header.fieldMarshaledSizesOffset = TMetaStream.ReadInt32();
                m_Header.fieldMarshaledSizesCount = TMetaStream.ReadInt32();
                m_Header.parametersOffset = TMetaStream.ReadInt32();
                m_Header.parametersCount = TMetaStream.ReadInt32();
                m_Header.fieldsOffset = TMetaStream.ReadInt32();
                m_Header.fieldsCount = TMetaStream.ReadInt32();
                m_Header.genericParametersOffset = TMetaStream.ReadInt32();
                m_Header.genericParametersCount = TMetaStream.ReadInt32();
                m_Header.genericParameterConstraintsOffset = TMetaStream.ReadInt32();
                m_Header.genericParameterConstraintsCount = TMetaStream.ReadInt32();
                m_Header.genericContainersOffset = TMetaStream.ReadInt32();
                m_Header.genericContainersCount = TMetaStream.ReadInt32();
                m_Header.nestedTypesOffset = TMetaStream.ReadInt32();
                m_Header.nestedTypesCount = TMetaStream.ReadInt32();
                m_Header.interfacesOffset = TMetaStream.ReadInt32();
                m_Header.interfacesCount = TMetaStream.ReadInt32();
                m_Header.vtableMethodsOffset = TMetaStream.ReadInt32();
                m_Header.vtableMethodsCount = TMetaStream.ReadInt32();
                m_Header.interfaceOffsetsOffset = TMetaStream.ReadInt32();
                m_Header.interfaceOffsetsCount = TMetaStream.ReadInt32();
                m_Header.typeDefinitionsOffset = TMetaStream.ReadInt32();
                m_Header.typeDefinitionsCount = TMetaStream.ReadInt32();
                m_Header.rgctxEntriesOffset = TMetaStream.ReadInt32();
                m_Header.rgctxEntriesCount = TMetaStream.ReadInt32();
                m_Header.imagesOffset = TMetaStream.ReadInt32();
                m_Header.imagesCount = TMetaStream.ReadInt32();
                m_Header.assembliesOffset = TMetaStream.ReadInt32();
                m_Header.assembliesCount = TMetaStream.ReadInt32();
                m_Header.metadataUsageListsOffset = TMetaStream.ReadInt32();
                m_Header.metadataUsageListsCount = TMetaStream.ReadInt32();
                m_Header.metadataUsagePairsOffset = TMetaStream.ReadInt32();
                m_Header.metadataUsagePairsCount = TMetaStream.ReadInt32();
                m_Header.fieldRefsOffset = TMetaStream.ReadInt32();
                m_Header.fieldRefsCount = TMetaStream.ReadInt32();
                m_Header.referencedAssembliesOffset = TMetaStream.ReadInt32();
                m_Header.referencedAssembliesCount = TMetaStream.ReadInt32();
                m_Header.attributesInfoOffset = TMetaStream.ReadInt32();
                m_Header.attributesInfoCount = TMetaStream.ReadInt32();
                m_Header.attributeTypesOffset = TMetaStream.ReadInt32();
                m_Header.attributeTypesCount = TMetaStream.ReadInt32();
                m_Header.unresolvedVirtualCallParameterTypesOffset = TMetaStream.ReadInt32();
                m_Header.unresolvedVirtualCallParameterTypesCount = TMetaStream.ReadInt32();
                m_Header.unresolvedVirtualCallParameterRangesOffset = TMetaStream.ReadInt32();
                m_Header.unresolvedVirtualCallParameterRangesCount = TMetaStream.ReadInt32();
                m_Header.windowsRuntimeTypeNamesOffset = TMetaStream.ReadInt32();
                m_Header.windowsRuntimeTypeNamesSize = TMetaStream.ReadInt32();
                m_Header.exportedTypeDefinitionsOffset = TMetaStream.ReadInt32();
                m_Header.exportedTypeDefinitionsCount = TMetaStream.ReadInt32();

                TMetaStream.Seek(m_Header.stringOffset + 4, SeekOrigin.Begin);

                var lpStrings = TMetaStream.ReadBytes(m_Header.stringCount);

                UInt32 dwMagicCheck = BitConverter.ToUInt32(lpStrings, 0);

                if (dwMagicCheck == 0x975ED124)
                {
                    lpStrings = Cipher.iDecryptData(lpStrings);

                    TMetaStream.Seek(m_Header.stringOffset + 4, SeekOrigin.Begin);
                    TMetaStream.Write(lpStrings, 0, lpStrings.Length);
                    TMetaStream.Seek(4, SeekOrigin.Begin);

                    lpBuffer = TMetaStream.ReadBytes((Int32)TMetaStream.Length - 4);
                    File.WriteAllBytes(m_MetaFileOut, lpBuffer);
                }

                TMetaStream.Dispose();
            }
        }
    }
}
