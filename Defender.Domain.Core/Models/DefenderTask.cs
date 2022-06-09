using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Defender.Domain.Core.Models;

public class DefenderTask
{
    public DefenderTask(string directory)
    {
        Directory = directory;
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Directory { get; set; }
    public DefenderTaskStatus Status { get; set; } = DefenderTaskStatus.Created;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int Errors { get; set; }
    public int FilesProcessed { get; set; }
    
    public int JsDetected { get; set; }
    public int RmRfDetected { get; set; }
    public int RunDllDetected { get; set; }
    
    public string Error { get; set; }
}

[StronglyTypedId(jsonConverter: StronglyTypedIdJsonConverter.NewtonsoftJson)]
public partial struct TaskId
{

}