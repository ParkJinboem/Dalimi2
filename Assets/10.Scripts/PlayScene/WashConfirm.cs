using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashConfirm : MonoBehaviour
{
    private List<Color> correctColor;

    public bool WashConfirms(PaintTools washTool, AdvancedMobilePaint.AdvancedMobilePaint paint, Wash wash)
    {
        correctColor = new List<Color>();
        Color[] pinatTex = paint.tex.GetPixels();

        for (int i = 0; i < pinatTex.Length; i++)
        {
            if(pinatTex[i].r == 1 && pinatTex[i].g == 1 && pinatTex[i].b == 1 && washTool != PaintTools.Towel)
            {
                pinatTex[i] = new Color(0, 0, 0, 0);
            }
        }

        switch (washTool)
        {
            case PaintTools.Soap:
                
                Color[] bubbleTex = wash.bubbleMask.GetPixels();

                for (int i = 0; i < pinatTex.Length; i++)
                {
                    if (pinatTex[i] == bubbleTex[i])
                    {
                        correctColor.Add(pinatTex[i]);
                    }
                }

                break;
            case PaintTools.Shower:
                Color[] showerTex = wash.dropMask.GetPixels();

                for (int i = 0; i < pinatTex.Length; i++)
                {
                    if (pinatTex[i] == showerTex[i])
                    {
                        correctColor.Add(pinatTex[i]);
                    }
                }
                
                break;
            case PaintTools.Towel:
                Color[] towelTex = wash.clearTex.GetPixels();

                for (int i = 0; i < pinatTex.Length; i++)
                {
                    if (pinatTex[i] == towelTex[i])
                    {
                        correctColor.Add(pinatTex[i]);
                    }
                }
                
                break;
            case PaintTools.MassagePack:
                Color[] massagePackTex = wash.greenMask.GetPixels();

                for (int i = 0; i < pinatTex.Length; i++)
                {
                    if (pinatTex[i] == massagePackTex[i])
                    {
                        correctColor.Add(pinatTex[i]);
                    }
                }
                
                break;
            case PaintTools.Mask:
                //별도의 스크립트로 구현
                break;
            case PaintTools.Cucumber:
                //별도의 스크립트로 구현
                break;
            case PaintTools.Cream:
                Color[] creamTex = wash.creamMask.GetPixels();

                for (int i = 0; i < pinatTex.Length; i++)
                {
                    if (pinatTex[i] == creamTex[i])
                    {
                        correctColor.Add(pinatTex[i]);
                    }
                }
                
                break;

            case PaintTools.Cheek:
                Color[] cheeKTex = wash.cheeks[wash.cheekNum].GetPixels();

                for (int i = 0; i < pinatTex.Length; i++)
                {
                    if (pinatTex[i] == cheeKTex[i])
                    {
                        correctColor.Add(pinatTex[i]);
                    }
                }
                if(correctColor.Count > Statics.correctCount)
                {
                    wash.firstCheek = true;
                }
                break;
        }

        if(correctColor.Count > Statics.correctCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
