using System.Collections.Generic;
using System.Xml.Serialization;
public class StoredProcedureEntity
{
    [XmlAttribute()]
    public string StoredProcedureName { get; set; }

    [XmlArray("StoredProceduresParameter")]
    public List<StoredProceduresParameterEntity> StoredProceduresParameter = new List<StoredProceduresParameterEntity>();
}