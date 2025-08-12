using DicountGrpc.Data;
using DicountGrpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DicountGrpc.Services
{
    public class DiscountService(DiscountDbContext dbContext) :DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
            if (coupon is null) 
                coupon=new  Coupon{ProductName = "No Coupon",Amount = 0,Description = "No Coupon for Desc"};

            return  coupon.Adapt<CouponModel>();
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            Coupon? coupon = request.Coupon.Adapt<DicountGrpc.Models.Coupon>();
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));

            dbContext.Coupons.Add(coupon);
            await dbContext.SaveChangesAsync();

            return coupon.Adapt<CouponModel>();
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));


            dbContext.Coupons.Update(coupon);
            await dbContext.SaveChangesAsync();
            return coupon.Adapt<CouponModel>();
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
            if (coupon is null)            
                throw new RpcException(new Status(StatusCode.NotFound, $"Not found {request.ProductName}"));
            
            dbContext.Coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();

            return new DeleteDiscountResponse { Success=true };
        }
    }
}
