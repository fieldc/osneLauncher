namespace osnE.Verbs
{
    public class SentieoVerb  : BaseUrlOpenerVerb
    {
        public SentieoVerb() :
            base("sen", 
                "Open Sentieo equity data terminal", 
                "company ticker",
                "https://app.sentieo.com/#company?ticker={0}&initial=true&selected=summary&token_label={0}&id=5&cst=true")
        {
            
        }
        
        
    }
}