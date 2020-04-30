using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace DropDownCompenent {
    public class DropdownState {
        public Stack<RenderFragment> Content { get; private set; } = new Stack<RenderFragment>();
        public bool OpenState {get; set;} = false;

        public event Action OnChange;

        public void AddRenderfragment (RenderFragment fragment) {
            Content.Push (fragment);
            NotifyStateChanged ();
        }

        public void Quit() {
            OpenState = false;
            NotifyStateChanged ();
        }

        public void PopRenderFragment () {
            Content.Pop ();
        }

        public void ClearContent() {
            Content.Clear();
        }

        private void NotifyStateChanged () => OnChange?.Invoke ();
    }
}