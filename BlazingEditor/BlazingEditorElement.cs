using System.Collections.Generic;

namespace BlazingEditor
{
    public class BlazingRender {
        public List<BlazingEditorElement> blocks {get; set;}
        public long time {get; set;}

        public string version {get;set;}
    }
    public class BlazingEditorElement
    {
        public string type {get; set;}
        public Dictionary<string, object> data {get; set;}

    }
}