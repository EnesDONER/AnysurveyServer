using Core.Utilities.Results;


namespace Business.Abstract
{
    public interface IPaymentService
    {
        IResult Pay(int userId, int cardId, decimal amount);
    }
}
