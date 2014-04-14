using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax.Data.Models
{
    public class BugPost
    {
        public BugPost()
        {
            this.Id = Guid.NewGuid();
            this.Timestamp = DateTime.Now;
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }

        //The full name of the FogBugz user the case creation or edit should be made as. 
        //We recommend using a Virtual User for this.
        public string UserName { get; set; }

        //The Project that new cases should be created in (must be a valid project name). 
        //Note that if BugzScout appends to an existing case, this field is ignored.
        public string ProjectName { get; set; }

        //The Area that new cases should go into (must be a valid area in the ScoutProject). 
        //Note that if BugzScout appends to an existing case, this field is ignored.
        public string AreaName { get; set; }

        //This is the unique string that identifies the particular crash that has just occurred. 
        //BugzScout submissions will be consolidated into one case based on the Description. 
        //When a BugzScout submission arrives in FogBugz, if there are no existing cases with 
        //the exact same Description, then a new case is created in the ScoutProject and ScoutArea. 
        //If a case is found with a matching description, the new submission is APPENDED to that 
        //case’s history, and a new case will NOT be created in FogBugz. When appending to a case, 
        //the ScoutProject and ScoutArea fields are ignored.

        //Because BugzScout submissions are automatically consolidated, it is important to include 
        //appropriate information when constructing the Description. We always include the short 
        //error message that was generated, and then some additional information. For example, 
        //it is usually wise to include the application version number so that a similar crash 
        //in version 1.1 and in version 1.2 will NOT append to the same case. You may consider 
        //including the OS that the crash occurred on, if relevant.
        //Here is a good starting point:
        //MyAppName X.Y.Z SomekindofException: Exception message text here 
        //which yields something like this actual error from part of the FogBugz javascript code: 
        //
        //FogBugz-8.9.72.0H: JS Error: TypeError: 'undefined' is not an object (evaluating 'bug.ixBug')
        //
        public string Description { get; set; }

        //The details about this particular crash. This is often a good place to put a stack trace, 
        //operating environment information, HTTP headers, or other details about what might have 
        //caused the error. Include as much information as you can, but beware of sending sensitive
        //information. For example, in billing code, make sure your exception handling code strips 
        //out credit card information.
        public string ExtraInformation { get; set; }

        //An email address to associate with the report, often the customer’s email. 
        //This overwrites the correspondent field on the case with each appended occurrence, 
        //so it is automatically included at the end of the case event as well.
        public string EmailAddress { get; set; }

        //An option to override the normal automatic consolidation of BugzScout reports. 
        //If this is set to “1″ or “true”, then a new case will always created from this submission.
        public bool IsForceNewBug { get; set; }

        // ezt is kihagyjuk, most nincs rá szükség
        //This is the default text that will be returned by the HTTP post request. 
        //If the submission is appended to an existing case, and that case has some 
        //text in the “Scout Message” field, then that text will be returned instead. 
        //This is useful when you want to let your first user experiencing a crash 
        //know that “we are investigating the issue,” but update that message in the 
        //case later on when you know that “this problem is fixed in the next version.
        //public string DefaultMessage { get; set; }

        //Set to “1″ or “true” in order to get a response in HTML format. By default, the response will be XML.
        public bool IsFriendlyResponse { get; set; }
    }
}
