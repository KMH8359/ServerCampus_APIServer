using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PvPGameServer.Room;

namespace PvPGameServer;

public class OmokGame
{
    public enum 돌종류 { 없음, 흑돌, 백돌 };

    const int 바둑판크기 = 19;


    int[,] 바둑판 = new int[바둑판크기, 바둑판크기];

    OmokGame()
    {

    }

    public void BoardClear()
    {
        Array.Clear(바둑판, 0, 바둑판크기 * 바둑판크기);
    }

    public void PutMok(int posX, int posY, 돌종류 돌)
    {
        바둑판[posX, posY] = (int)돌;
    }

    public int CheckOmokBoardPosition(int posX, int posY)
    {
        return 바둑판[posX, posY];
    }
    public bool CheckWinCondition(int x, int y)
    {
        if (가로확인(x, y) == 5)
        {
            return true;
        }

        else if (세로확인(x, y) == 5)
        {
            return true;
        }

        else if (사선확인(x, y) == 5)
        {
            return true;
        }

        else if (역사선확인(x, y) == 5)
        {
            return true;
        }

        return false;
    }

    int 가로확인(int x, int y)
    {
        int 같은돌개수 = 1;

        for (int i = 1; i <= 5; i++)
        {
            if (x + i <= 18 && 바둑판[x + i, y] == 바둑판[x, y])
                같은돌개수++;

            else
                break;
        }

        for (int i = 1; i <= 5; i++)
        {
            if (x - i >= 0 && 바둑판[x - i, y] == 바둑판[x, y])
                같은돌개수++;

            else
                break;
        }

        return 같은돌개수;
    }

    int 세로확인(int x, int y)
    {
        int 같은돌개수 = 1;

        for (int i = 1; i <= 5; i++)
        {
            if (y + i <= 18 && 바둑판[x, y + i] == 바둑판[x, y])
                같은돌개수++;

            else
                break;
        }

        for (int i = 1; i <= 5; i++)
        {
            if (y - i >= 0 && 바둑판[x, y - i] == 바둑판[x, y])
                같은돌개수++;

            else
                break;
        }

        return 같은돌개수;
    }

    int 사선확인(int x, int y)
    {
        int 같은돌개수 = 1;

        for (int i = 1; i <= 5; i++)
        {
            if (x + i <= 18 && y - i >= 0 && 바둑판[x + i, y - i] == 바둑판[x, y])
                같은돌개수++;

            else
                break;
        }

        for (int i = 1; i <= 5; i++)
        {
            if (x - i >= 0 && y + i <= 18 && 바둑판[x - i, y + i] == 바둑판[x, y])
                같은돌개수++;

            else
                break;
        }

        return 같은돌개수;
    }

    int 역사선확인(int x, int y)
    {
        int 같은돌개수 = 1;

        for (int i = 1; i <= 5; i++)
        {
            if (x + i <= 18 && y + i <= 18 && 바둑판[x + i, y + i] == 바둑판[x, y])
                같은돌개수++;

            else
                break;
        }

        for (int i = 1; i <= 5; i++)
        {
            if (x - i >= 0 && y - i >= 0 && 바둑판[x - i, y - i] == 바둑판[x, y])
                같은돌개수++;

            else
                break;
        }

        return 같은돌개수;
    }

    public bool 삼삼확인(int x, int y)     // 33확인
    {
        int 삼삼확인 = 0;

        삼삼확인 += 가로삼삼확인(x, y);
        삼삼확인 += 세로삼삼확인(x, y);
        삼삼확인 += 사선삼삼확인(x, y);
        삼삼확인 += 역사선삼삼확인(x, y);

        if (삼삼확인 >= 2)
            return true;

        else
            return false;
    }

    int 가로삼삼확인(int x, int y)    // 가로 (ㅡ) 확인
    {
        int 돌3개확인 = 1;
        int i, j;

        for (i = 1; i <= 3; i++) // 돌을 둔 위치로부터 → 확인
        {
            if (x + i > 18)
                break;

            else if (바둑판[x + i, y] == 바둑판[x, y])
                돌3개확인++;

            else if (바둑판[x + i, y] != (int)돌종류.없음)
                break;
        }

        for (j = 1; j <= 3; j++) // 돌을 둔 위치로부터 ← 확인
        {
            if (x - j < 0)
                break;

            else if (바둑판[x - j, y] == 바둑판[x, y])
                돌3개확인++;

            else if (바둑판[x - j, y] != (int)돌종류.없음)
                break;
        }

        if (돌3개확인 == 3 && x + i < 18 && x - j > 0)    //돌 개수가 3개면서 양쪽 벽에 붙어잇으면 안된다
        {
            if ((바둑판[x + i, y] != (int)돌종류.없음 && 바둑판[x + i - 1, y] != (int)돌종류.없음) || (바둑판[x - j, y] != (int)돌종류.없음 && 바둑판[x - j + 1, y] != (int)돌종류.없음))
            {
                return 0;
            }

            else
                return 1;
        }

        return 0;
    }

    private int 세로삼삼확인(int x, int y)    // 세로 (|) 확인
    {
        int 돌3개확인 = 1;
        int i, j;

        돌3개확인 = 1;

        for (i = 1; i <= 3; i++) // 돌을 둔 위치로부터 ↓ 확인
        {
            if (y + i > 18)
                break;

            else if (바둑판[x, y + i] == 바둑판[x, y])
                돌3개확인++;

            else if (바둑판[x, y + i] != (int)돌종류.없음)
                break;
        }

        for (j = 1; j <= 3; j++) // 돌을 둔 위치로부터 ↑ 확인
        {
            if (y - j < 0)
                break;

            else if (바둑판[x, y - j] == 바둑판[x, y])
                돌3개확인++;

            else if (바둑판[x, y - j] != (int)돌종류.없음)
                break;
        }

        if (돌3개확인 == 3 && y + i < 18 && y - j > 0)    //돌 개수가 3개면서 양쪽 벽에 붙어잇으면 안된다
        {
            if ((바둑판[x, y + i] != (int)돌종류.없음 && 바둑판[x, y + i - 1] != (int)돌종류.없음) || (바둑판[x, y - j] != (int)돌종류.없음 && 바둑판[x, y - j + 1] != (int)돌종류.없음))
            {
                return 0;
            }

            else
                return 1;
        }

        return 0;
    }

    int 사선삼삼확인(int x, int y)    // 사선 (/) 확인
    {
        int 돌3개확인 = 1;
        int i, j;

        돌3개확인 = 1;

        for (i = 1; i <= 3; i++) // 돌을 둔 위치로부터 ↗ 확인
        {
            if (x + i > 18 || y - i < 0)
                break;

            else if (바둑판[x + i, y - i] == 바둑판[x, y])
                돌3개확인++;

            else if (바둑판[x + i, y - i] != (int)돌종류.없음)
                break;
        }

        for (j = 1; j <= 3; j++) // 돌을 둔 위치로부터 ↙ 확인
        {
            if (x - j < 0 || y + j > 18)
                break;

            else if (바둑판[x - j, y + j] == 바둑판[x, y])
                돌3개확인++;

            else if (바둑판[x - j, y + j] != (int)돌종류.없음)
                break;
        }

        if (돌3개확인 == 3 && x + i < 18 && y - i > 0 && x - j > 0 && y + j < 18)    //돌 개수가 3개면서 양쪽 벽에 붙어잇으면 안된다
        {
            if ((바둑판[x + i, y - i] != (int)돌종류.없음 && 바둑판[x + i - 1, y - i + 1] != (int)돌종류.없음) || (바둑판[x - j, y + j] != (int)돌종류.없음 && 바둑판[x - j + 1, y + j - 1] != (int)돌종류.없음))
            {
                return 0;
            }

            else
                return 1;
        }

        return 0;
    }

    int 역사선삼삼확인(int x, int y)    // 역사선 (＼) 확인
    {
        int 돌3개확인 = 1;
        int i, j;

        돌3개확인 = 1;

        for (i = 1; i <= 3; i++) // 돌을 둔 위치로부터 ↘ 확인
        {
            if (x + i > 18 || y + i > 18)
                break;

            else if (바둑판[x + i, y + i] == 바둑판[x, y])
                돌3개확인++;

            else if (바둑판[x + i, y + i] != (int)돌종류.없음)
                break;
        }

        for (j = 1; j <= 3; j++) // 돌을 둔 위치로부터 ↖ 확인
        {
            if (x - j < 0 || y - j < 0)
                break;

            else if (바둑판[x - j, y - j] == 바둑판[x, y])
                돌3개확인++;

            else if (바둑판[x - j, y - j] != (int)돌종류.없음)
                break;
        }

        if (돌3개확인 == 3 && x + i < 18 && y + i < 18 && x - j > 0 && y - j > 0)    //돌 개수가 3개면서 양쪽 벽에 붙어잇으면 안된다
        {
            if ((바둑판[x + i, y + i] != (int)돌종류.없음 && 바둑판[x + i - 1, y + i - 1] != (int)돌종류.없음) || (바둑판[x - j, y - j] != (int)돌종류.없음 && 바둑판[x - j + 1, y - j + 1] != (int)돌종류.없음))
            {
                return 0;
            }

            else
                return 1;
        }

        return 0;
    }
}

