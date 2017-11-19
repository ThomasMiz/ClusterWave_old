using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using ClusterWave.UI.Elements;
using ClusterWave.UI;

namespace ClusterWave.UI.MenuManager
{
    class WindowManager
    {
        static List<Window> winOrder;
        //public Vector2 mousePos;

        int e = 0;
        List<Window> winOrderCopy;

        public WindowManager()
        {
            winOrder = new List<Window>(50);
            winOrderCopy = new List<Window>(50);
        }
        public void WindowUpdate(Vector2 mousePos)
        {
            foreach (Window w in winOrder)
            {
                if (w != null)
                    if(w.isFocused && w.isMouseDown)
                        w.Update(mousePos);
            }
        }
        public void WindowAdd(Window w, Menu m)
        {
            
            //winOrder[0] = w;
            winOrder.Add(w);
            m.Add(w.d.t);
            m.Add(w.d.b);
            m.Add(w.d.btn);
            //winOrder.Insert(0, w);
            e++;
            //Game1.game.Window.Title += w.name + "--";
            
            
        }
        public void WindowBringFront(Window w)
        {
            winOrderCopy = winOrder;
            /*
            //winOrder[winOrder.Length].Close();
            if(winOrder[0] != null && winOrder[0].isFocused)
                winOrder[0].isFocused = false;
            for (int i=0; i < winOrder.Count-1; i++)
            {
                winOrder[i+1] = newWinOrder[i];
            }
            winOrder[0] = w;*/
            //winOrder.Remove(w);
            /*for (int i = 1; i<50; i++){
                if(winOrder[i] != null)
                    winOrder.Insert(i, winOrder[i-1]);
            }*/
            if (winOrder[0] != w)
            {
                winOrder[0].isFocused = false;
                for (int i = winOrder.Count - 1; i > 0; i--)
                {
                    winOrder[i] = winOrder[i - 1];
                }
                winOrder.Insert(0, w);
            }

            
        }
        public void Draw(SpriteBatch batch, GraphicsDevice device, Vector2 mousePos)
        {
            for (int i = winOrder.Count-1; i >=0; i--)
            {
                if(winOrder[i] != null)
                    winOrder[i].Draw(batch, device, mousePos);
            }
          
        }
    }
}












/*
*
*
*

 public void WindowAdd(Window w, Menu m)
        {
            //winOrder[0] = w;

            m.Add(w.d.t);
            m.Add(w.d.b);
            m.Add(w.d.btn);

            //Game1.game.Window.Title += w.name + "--";
            
            WindowBringFront(w);
        }
        public void WindowBringFront(Window w)
        {
            Window[] newWinOrder = winOrder;
            //winOrder[winOrder.Length].Close();
            if(winOrder[0] != null && winOrder[0].isFocused)
                winOrder[0].isFocused = false;
            for (int i=0; i < winOrder.Length-1; i++)
            {
                winOrder[i+1] = newWinOrder[i];
            }
            winOrder[0] = w;
        }

*
*
*
*/