#nullable enable
namespace hb.SbsdbServer.Model.ViewModel; 

public class EditExtprogTransport {
    public ExtProgList? In {get; set; }
    public ExtProgList? OutChg { get; set; }
    public ExtProgList? OutNew { get; set; }
    public ExtProgList? OutDel { get; set; }
}
