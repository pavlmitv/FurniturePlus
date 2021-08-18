using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurniturePlus.Services.Vendors
{
    public interface IVendorService
    {
        void Approve(int vendorId);
        void Decline(int vendorId);
    }
}
