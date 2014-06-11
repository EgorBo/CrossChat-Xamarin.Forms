//
// Bubble.cs: Provides both a UITableViewCell that can be used with UITableViews
// as well as a ChatBubble which is a MonoTouch.Dialog Element that can be used
// inside a DialogViewController for quick UIs.
//
// Author:
//   Miguel de Icaza
//

using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Crosschat.Client.iOS.CustomRenderers
{
    public class ChatBubble : Element, IElementSizing
    {
        bool isLeft;

        public ChatBubble(bool isLeft, string text)
            : base(text)
        {
            this.isLeft = isLeft;
        }


        public override UITableViewCell GetCell(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(isLeft ? BubbleCell.KeyLeft : BubbleCell.KeyRight) as BubbleCell;
            if (cell == null)
                cell = new BubbleCell(isLeft);
            cell.Update(Caption);
            return cell;
        }

        public float GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            return BubbleCell.GetSizeForText(tableView, Caption).Height + BubbleCell.BubblePadding.Height;
        }
    }
}
