using System;
namespace ummisco.gama.unity.littosim
{
    public class Action
    {
        public string action_name; // { get; set; }
        public int action_code; // { get; set; }
        public int delay; // { get; set; }
        public float cost; // { get; set; }
        public string entity; // { get; set; }
        public string button_help_message; // { get; set; }
        public string button_icon_file; // { get; set; }
        public int coast_def_index; //{ get; set; }
        public int lu_index; // { get; set; }

        public Action()
        {

        }

        public Action(string action_name, int action_code, int delay, float cost, string entity, string button_help_message, string button_icon_file, int coast_def_index, int lu_index)
        {
            this.action_name = action_name;
            this.action_code = action_code;
            this.button_help_message = button_help_message;
            this.button_icon_file = button_icon_file;
            this.coast_def_index = coast_def_index;
            this.lu_index = lu_index;
        }
    }
}
