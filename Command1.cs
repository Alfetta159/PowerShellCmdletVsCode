using System.Management.Automation;

namespace MeyerCorp.MyCmdletProject;

[Cmdlet(VerbsCommunications.Write, "Demo")]
public class SendStandardEmail : Cmdlet
{
    [Parameter(Mandatory = true)]
    public string Subject { get; set; } = default!;

    [Parameter(Mandatory = false)]
    public string Reason { get; set; } = default!;

    [Parameter(Mandatory = true)]
    public string What { get; set; } = default!;

    [Parameter(Mandatory = false)]
    public string Why { get; set; } = default!;

    [Parameter(Mandatory = true)]
    public string[] Tos { get; set; } = default!;

    [Parameter(Mandatory = false)]
    public string[] Ccs { get; set; } = default!;

    [Parameter(Mandatory = false)]
    public string[] Bccs { get; set; } = default!;

    protected override void ProcessRecord()
    {
        WriteObject("Hello world!");
        WriteObject("______________________________________");

        WriteObject($"To: {String.Join(", ", Tos)}");
        WriteObject($"cc: {(Ccs == null ? String.Empty : String.Join(", ", Ccs))}");
        WriteObject($"bcc: {(Bccs == null ? String.Empty : String.Join(", ", Bccs))}");
        WriteObject(String.Empty);
        WriteObject(Subject);
        WriteObject(String.Empty);
        WriteObject(What);
        WriteObject(String.Empty);
        WriteObject(Reason);
        WriteObject(String.Empty);
        WriteObject(Why);
        WriteObject(String.Empty);
        WriteObject("That is all.");
        WriteObject(String.Empty);
        WriteObject("______________________________________");
        WriteObject("Goodbye cruel world!");
    }
}
