﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WatchStore.Domain;
using WatchStore.Domain.Dto;
using WatchStore.Repository;
using WatchStore.Service.Interface;

namespace WatchStore.Service
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<ProductInOrder> _productInOrderRepository;
        private readonly IUserRepository _userRepository;


        public ShoppingCartService(IRepository<Order> orderRepository, IRepository<ProductInOrder> productInOrderRepository, IUserRepository userRepository)
        {
            this._orderRepository = orderRepository;
            _productInOrderRepository = productInOrderRepository;
            _userRepository = userRepository;
        }



        public ShoppingCartDto getShoppingCartInfo(User u)
        {
            if (u != null)
            {

                var userCard = u.UserCart;

                var allProducts = userCard.ProductInShoppingCarts.ToList();
                List<ProductInShoppingCartDto> allProductDto = new List<ProductInShoppingCartDto>();
                foreach (var product in allProducts)
                {
                    ProductInShoppingCartDto productInShoppingCartDto = new ProductInShoppingCartDto
                    {
                        id = product.CurrnetProduct.Id,
                        ProductDescription = product.CurrnetProduct.ProductDescription,
                        ProductImage = product.CurrnetProduct.ProductImage,
                        ProductName = product.CurrnetProduct.ProductName,
                        ProductPrice = product.CurrnetProduct.ProductPrice,
                        quantity = product.Quantity
                    };
                    allProductDto.Add(productInShoppingCartDto);
                }
                
            

                var allProductPrices = allProducts.Select(z => new
                {
                    ProductPrice = z.CurrnetProduct.ProductPrice,
                    Quantity = z.Quantity
                }).ToList();

                double totalPrice = 0.0;

                foreach (var item in allProductPrices)
                {
                    totalPrice += item.Quantity * item.ProductPrice;
                }

                var reuslt = new ShoppingCartDto
                {
                    Products = allProductDto,
                    TotalPrice = totalPrice
                };

                return reuslt;
            }
            return new ShoppingCartDto();
        }

        public bool order(User u)
        {
            if (u!=null)
            {
                
                var userCard = u.UserCart;

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    User = u,
                    UserId = u.Id.ToString(),
                };

                this._orderRepository.Insert(order);

                List<ProductInOrder> productInOrders = new List<ProductInOrder>();

                var result = userCard.ProductInShoppingCarts.Select(z => new ProductInOrder
                {
                    Id = Guid.NewGuid(),
                    ProductId = z.CurrnetProduct.Id,
                    Product = z.CurrnetProduct,
                    OrderId = order.Id,
                    Order = order,
                    Quantity = z.Quantity
                }).ToList();

                productInOrders.AddRange(result);

                foreach (var item in productInOrders)
                {
                    this._productInOrderRepository.Insert(item);
                }

                u.UserCart.ProductInShoppingCarts.Clear();

                this._userRepository.Update(u);

                return true;
            }

            return false;
        }
    }
}
