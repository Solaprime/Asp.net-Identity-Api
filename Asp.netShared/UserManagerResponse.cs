using System;
using System.Collections.Generic;
using System.Text;

namespace Asp.netShared
{
    // yhis class will basically wrap our resppnse 
    // the response that will be sent to the user 
    // 
    public class UserManagerResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Error { get; set; }
        // Additonal Property for when the token
        // will expired
        public DateTime? ExpiredDate { get; set; }
    }
}
