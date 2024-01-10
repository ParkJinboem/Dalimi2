using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PaintUtils : MonoBehaviour
{
	public static Texture2D RotateTexture(Texture2D tex, float angle)
	{
		Texture2D rotImage = new Texture2D(tex.width, tex.height, tex.format, false);
		int x, y;
		float x1, y1, x2, y2;

		int w = tex.width;
		int h = tex.height;
		float x0 = rot_x(angle, -w / 2.0f, -h / 2.0f) + w / 2.0f;
		float y0 = rot_y(angle, -w / 2.0f, -h / 2.0f) + h / 2.0f;

		float dx_x = rot_x(angle, 1.0f, 0.0f);
		float dx_y = rot_y(angle, 1.0f, 0.0f);
		float dy_x = rot_x(angle, 0.0f, 1.0f);
		float dy_y = rot_y(angle, 0.0f, 1.0f);


		x1 = x0;
		y1 = y0;
		Color32[] pixels = tex.GetPixels32(0);
		Color32[] result = new Color32[pixels.Length];
		Color c;
		int pixX = 0;
		int pixY = 0;
		//			int pixelPos=0;
		for (x = 0; x < tex.width; x++)
		{
			x2 = x1;
			y2 = y1;
			for (y = 0; y < tex.height; y++)
			{
				//rotImage.SetPixel (x1, y1, Color.clear);          

				x2 += dx_x;//rot_x(angle, x1, y1);
				y2 += dx_y;//rot_y(angle, x1, y1);
						   //
				pixX = (int)Mathf.Floor(x2);
				pixY = (int)Mathf.Floor(y2);
				if (pixX >= tex.width || pixX < 0 || pixY >= tex.height || pixY < 0)
				{
					c = Color.clear;
				}
				else
				{
					c = (Color)pixels[pixX * w + pixY];// = tex.GetPixel(x1,y1);
				}
				//
				//rotImage.SetPixel ( (int)Mathf.Floor(x), (int)Mathf.Floor(y), /*c*/getPixel(tex,x2, y2));
				result[(int)Mathf.Floor(x) * w + (int)Mathf.Floor(y)] = (Color32)c;
				//pixelPos++;
			}

			x1 += dy_x;
			y1 += dy_y;

		}
		rotImage.SetPixels32(result);
		rotImage.Apply();
		return rotImage;
	}

	private static float rot_x(float angle, float x, float y)
	{
		float cos = Mathf.Cos(angle / 180.0f * Mathf.PI);
		float sin = Mathf.Sin(angle / 180.0f * Mathf.PI);
		return (x * cos + y * (-sin));
	}
	private static float rot_y(float angle, float x, float y)
	{
		float cos = Mathf.Cos(angle / 180.0f * Mathf.PI);
		float sin = Mathf.Sin(angle / 180.0f * Mathf.PI);
		return (x * sin + y * cos);
	}



	public static Texture2D RotateTextureImage(Texture2D originTexture, int angle)
	{
		Texture2D result = new Texture2D(originTexture.width, originTexture.height);
		Color32[] pix1 = result.GetPixels32();
		Color32[] pix2 = originTexture.GetPixels32();
		int W = originTexture.width;
		int H = originTexture.height;
		int x = 0;
		int y = 0;
		Color32[] pix3 = rotateSquare(pix2, (Math.PI / 180 * (double)angle), originTexture);
		for (int j = 0; j < H; j++)
		{
			for (var i = 0; i < W; i++)
			{
				//pix1[result.width/2 - originTexture.width/2 + x + i + result.width*(result.height/2-originTexture.height/2+j+y)] = pix2[i + j*originTexture.width];
				pix1[result.width / 2 - W / 2 + x + i + result.width * (result.height / 2 - H / 2 + j + y)] = pix3[i + j * W];
			}
		}
		result.SetPixels32(pix1);
		result.Apply();
		return result;
	}

	//텍스쳐 회전코드 인터넷에서 퍼옴_230228 박진범
	static Color32[] rotateSquare(Color32[] arr, double phi, Texture2D originTexture)
	{
		int x;
		int y;
		int i;
		int j;
		double sn = Math.Sin(phi);
		double cs = Math.Cos(phi);
		Color32[] arr2 = originTexture.GetPixels32();
		int W = originTexture.width;
		int H = originTexture.height;
		int xc = W / 2;
		int yc = H / 2;
		for (j = 0; j < H; j++)
		{
			for (i = 0; i < W; i++)
			{
				arr2[j * W + i] = new Color32(0, 0, 0, 0);
				x = (int)(cs * (i - xc) + sn * (j - yc) + xc);
				y = (int)(-sn * (i - xc) + cs * (j - yc) + yc);
				if ((x > -1) && (x < W) && (y > -1) && (y < H))
				{
					arr2[j * W + i] = arr[y * W + x];
				}
			}
		}
		return arr2;
	}
}
