using System;

namespace ER.Metadata.Decryptor
{
    class Il2CppGlobalMetadataHeader
    {
        public Int32 magic { get; set; } // 0x504754 (TPG\0)
        public Int32 sanity { get; set; }
        public Int32 version { get; set; }
        public Int32 stringLiteralOffset { get; set; }
        public Int32 stringLiteralCount { get; set; }
        public Int32 stringLiteralDataOffset { get; set; }
        public Int32 stringLiteralDataCount { get; set; }
        public Int32 stringOffset { get; set; }
        public Int32 stringCount { get; set; }
        public Int32 eventsOffset { get; set; }
        public Int32 eventsCount { get; set; }
        public Int32 propertiesOffset { get; set; }
        public Int32 propertiesCount { get; set; }
        public Int32 methodsOffset { get; set; }
        public Int32 methodsCount { get; set; }
        public Int32 parameterDefaultValuesOffset { get; set; }
        public Int32 parameterDefaultValuesCount { get; set; }
        public Int32 fieldDefaultValuesOffset { get; set; }
        public Int32 fieldDefaultValuesCount { get; set; }
        public Int32 fieldAndParameterDefaultValueDataOffset { get; set; }
        public Int32 fieldAndParameterDefaultValueDataCount { get; set; }
        public Int32 fieldMarshaledSizesOffset { get; set; }
        public Int32 fieldMarshaledSizesCount { get; set; }
        public Int32 parametersOffset { get; set; }
        public Int32 parametersCount { get; set; }
        public Int32 fieldsOffset { get; set; }
        public Int32 fieldsCount { get; set; }
        public Int32 genericParametersOffset { get; set; }
        public Int32 genericParametersCount { get; set; }
        public Int32 genericParameterConstraintsOffset { get; set; }
        public Int32 genericParameterConstraintsCount { get; set; }
        public Int32 genericContainersOffset { get; set; }
        public Int32 genericContainersCount { get; set; }
        public Int32 nestedTypesOffset { get; set; }
        public Int32 nestedTypesCount { get; set; }
        public Int32 interfacesOffset { get; set; }
        public Int32 interfacesCount { get; set; }
        public Int32 vtableMethodsOffset { get; set; }
        public Int32 vtableMethodsCount { get; set; }
        public Int32 interfaceOffsetsOffset { get; set; }
        public Int32 interfaceOffsetsCount { get; set; }
        public Int32 typeDefinitionsOffset { get; set; }
        public Int32 typeDefinitionsCount { get; set; }
        public Int32 rgctxEntriesOffset { get; set; }
        public Int32 rgctxEntriesCount { get; set; }
        public Int32 imagesOffset { get; set; }
        public Int32 imagesCount { get; set; }
        public Int32 assembliesOffset { get; set; }
        public Int32 assembliesCount { get; set; }
        public Int32 metadataUsageListsOffset { get; set; }
        public Int32 metadataUsageListsCount { get; set; }
        public Int32 metadataUsagePairsOffset { get; set; }
        public Int32 metadataUsagePairsCount { get; set; }
        public Int32 fieldRefsOffset { get; set; }
        public Int32 fieldRefsCount { get; set; }
        public Int32 referencedAssembliesOffset { get; set; }
        public Int32 referencedAssembliesCount { get; set; }
        public Int32 attributesInfoOffset { get; set; }
        public Int32 attributesInfoCount { get; set; }
        public Int32 attributeTypesOffset { get; set; }
        public Int32 attributeTypesCount { get; set; }
        public Int32 unresolvedVirtualCallParameterTypesOffset { get; set; }
        public Int32 unresolvedVirtualCallParameterTypesCount { get; set; }
        public Int32 unresolvedVirtualCallParameterRangesOffset { get; set; }
        public Int32 unresolvedVirtualCallParameterRangesCount { get; set; }
        public Int32 windowsRuntimeTypeNamesOffset { get; set; }
        public Int32 windowsRuntimeTypeNamesSize { get; set; }
        public Int32 exportedTypeDefinitionsOffset { get; set; }
        public Int32 exportedTypeDefinitionsCount { get; set; }
    }
}
