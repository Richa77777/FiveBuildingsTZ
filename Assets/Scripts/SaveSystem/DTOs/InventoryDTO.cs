using System;
using System.Collections.Generic;

[Serializable]
public class InventoryDTO
{
    public List<ResourceEntryDTO> resources = new List<ResourceEntryDTO>();
}

[Serializable]
public class ResourceEntryDTO
{
    public ResourceType type;
    public int value;
}