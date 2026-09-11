using System.Collections.Generic;
namespace Models;

public interface IUser
{
    Guid UserId {get; set;}
    string User_Name {get; set;}

    List<IReview> Reviews {get; set;}
    
}