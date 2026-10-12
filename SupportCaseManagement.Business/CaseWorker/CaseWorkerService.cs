namespace SupportCaseManagement.Application.CaseWorker;

public class CaseWorkerService
{
    public List<string> GetAll()
    {
        List<string> caseWorkers = [];
        caseWorkers.Add("");    //ingen handläggare   
        caseWorkers.Add("Mia"); 
        caseWorkers.Add("Valerio");
        caseWorkers.Add("Tagiev");

        return caseWorkers;  
    }
}
