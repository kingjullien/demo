using System.Data;
using System.Xml.Serialization;
public class StoredProceduresParameterEntity
{
    public StoredProceduresParameterEntity(string ParameterName = "", string ParameterValue = "", SQLServerDatatype DataType = SQLServerDatatype.IntDataType)
    {
        _ParameterName = ParameterName;
        _ParameterValue = ParameterValue;
        _Datatype = Datatype;
    }

    public StoredProceduresParameterEntity(ParameterDirection Direction, string ParameterName = "", string ParameterValue = "", SQLServerDatatype DataType = SQLServerDatatype.IntDataType)
    {
        _ParameterName = ParameterName;
        _ParameterValue = ParameterValue;
        _Datatype = Datatype;
        _Direction = Direction;
    }

    public string _ParameterName { get; set; }
    public SQLServerDatatype _Datatype { get; set; }
    public string _ParameterValue { get; set; }
    private ParameterDirection _Direction { get; set; }

    [XmlAttribute()]
    public string ParameterName
    {
        get
        {
            return _ParameterName;
        }
        set
        {
            _ParameterName = value;
        }
    }
    [XmlAttribute()]
    public SQLServerDatatype Datatype
    {
        get
        {
            return _Datatype;
        }
        set
        {
            _Datatype = value;
        }
    }
    [XmlAttribute()]
    public string ParameterValue
    {
        get
        {
            return _ParameterValue;
        }
        set
        {
            _ParameterValue = value;
        }
    }

    [XmlAttribute()]
    public ParameterDirection Direction
    {
        get
        {
            return _Direction;
        }
        set
        {
            _Direction = value;
        }
    }
}



