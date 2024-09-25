

public class NetHelper
{
    public static bool CheckMessageSuccess(int code)
    {
        return code == GlobalData.SuccessCode;
    }
}