using System.ComponentModel.DataAnnotations;

namespace WebBanSach_2_0.Model.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "Đang chờ được duyệt")]
        Waiting,
        [Display(Name = "Đã được duyệt")]
        Accepted,
        [Display(Name = "Đang xếp hàng hóa")]
        InProgress,
        [Display(Name = "Đang vận chuyển")]
        Shipping,
        [Display(Name = "Đã nhận hàng")]
        Deliveried,
        [Display(Name = "Đã hoàn thành")]
        Completed,
        [Display(Name = "Đã bị hủy")]
        Cancelled,
        [Display(Name = "Đã bị từ chối")]
        Declined,
        [Display(Name = "Đã bị hoàn trả")]
        Refunded,
        [Display(Name = "Đã bị xóa")]
        Deleted
    }
}
