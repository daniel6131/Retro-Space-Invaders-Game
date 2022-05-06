using System.Collections;
using System.Collections.Generic;
using UnityEngine;
struct Achievement 
{
    private string _id;
    private string _title;

    public Achievement(string ID)
    {
        this._id = ID;

        switch (_id)
        {
            case "alien_kill_1":
                _title = "Alien Murderer";
                break;
            case "alien_kill_50":
                _title = "Alien Dominator";
                break;
            case "alien_destroy_250":
                _title = "God Amongst Aliens";
                break;
            case "score_500":
                _title = "Rookie";
                break;
            case "score_1000":
                _title = "Skilled";
                break;
            case "score_2000":
                _title = "Kingly";
                break;
            case "mysteryShip_kill_1":
                _title = "Go Back Where You Came";
                break;
            case "mysteryShip_kill_3":
                _title = "They Keep Coming?";
                break;
            case "mysteryShip_kill_5":
                _title = "How Many Are There?";
                break;

            default:
                _title = null;
                break;
        }
    }

    public string GetID()
    {
        return _id;
    }

    public string GetTitle()
    {
        return _title;
    }
}
