using System;
using System.Collections.Generic;
using System.Drawing;

namespace ImageSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            string imagePath = "1.png";
            Bitmap image = new Bitmap(imagePath);
            int[,] binaryImage = ConvertToBinary(image, 127);

            List<List<Point>> areas = Search(binaryImage);
            Console.WriteLine(areas.Count);
        }

        static int[,] ConvertToBinary(Bitmap image, int threshold)
        {
            int width = image.Width;
            int height = image.Height;
            int[,] binaryImage = new int[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int grayScale = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

                    if (grayScale > threshold)
                        binaryImage[y, x] = 255;
                    else
                        binaryImage[y, x] = 0;
                }
            }

            return binaryImage;
        }

        static List<List<Point>> Search(int[,] image)
        {
            int height = image.GetLength(0);
            int width = image.GetLength(1);
            List<List<Point>> areas = new List<List<Point>>();
            bool[,] visited = new bool[height, width];
            int[][] offsets =
            {
                new int[] { -1, -1 }, new int[] { -1, 0 }, new int[] { -1, 1 },
                new int[] { 0, -1 },                      new int[] { 0, 1 },
                new int[] { 1, -1 },  new int[] { 1, 0 },  new int[] { 1, 1 }
            };

            List<Point> DFS(Point point)
            {
                Stack<Point> stack = new Stack<Point>();
                List<Point> points = new List<Point>();

                stack.Push(point);

                while (stack.Count > 0)
                {
                    Point current = stack.Pop();
                    int x = current.X;
                    int y = current.Y;
                    visited[x, y] = true;
                    points.Add(current);

                    foreach (int[] offset in offsets)
                    {
                        int dx = offset[0];
                        int dy = offset[1];
                        int nx = x + dx;
                        int ny = y + dy;

                        if (nx >= 0 && nx < height && ny >= 0 && ny < width)
                        {
                            if (image[nx, ny] != 255 && !visited[nx, ny])
                            {
                                stack.Push(new Point(nx, ny));
                            }
                        }
                    }
                }

                return points;
            }

            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    if (image[x, y] != 255 && !visited[x, y])
                    {
                        areas.Add(DFS(new Point(x, y)));
                    }
                }
            }

            return areas;
        }
    }
}
