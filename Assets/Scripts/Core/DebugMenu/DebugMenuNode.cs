using System;
using System.Collections.Generic;

using pdxpartyparrot.Core.UI;

using UnityEngine;

namespace pdxpartyparrot.Core.DebugMenu
{
    public sealed class DebugMenuNode
    {
        public Func<string> Title { get; }

        public DebugMenuNode Parent { get; }

        public Action RenderContentsAction { get; set; }

        private readonly List<DebugMenuNode> _children = new List<DebugMenuNode>();

        public DebugMenuNode(Func<string> title)
        {
            Title = title;
        }

        public DebugMenuNode(Func<string> title, DebugMenuNode parent)
        {
            Title = title;
            Parent = parent;
        }

        public void RenderNode()
        {
            string title = Title();
            if(GUILayout.Button(title, GUIUtils.GetLayoutButtonSize(title))) {
                DebugMenuManager.Instance.SetCurrentNode(this);
            }
        }

        public void RenderContents()
        {
            foreach(DebugMenuNode child in _children) {
                child.RenderNode();
            }

            RenderContentsAction?.Invoke();
        }

        public DebugMenuNode AddNode(Func<string> title)
        {
            DebugMenuNode node = new DebugMenuNode(title, this);
            _children.Add(node);
            return node;
        }
    }
}
