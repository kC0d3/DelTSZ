import { useState } from 'react';
import { toast } from 'react-toastify';

export default function ProductCard({ product, productTypes, fetchOwnerProductsAndIngredients }) {
    const [amount, setAmount] = useState('');
    const productType = productTypes.find(type => type.index === product.type);

    const handleOrder = async e => {
        e.preventDefault();

        try {
            const orderRes = await fetch(`/api/products/${e.target.parentNode.parentNode.id}/${Number(amount)}`, {
                method: 'PUT'
            });
            const orderData = await orderRes.json();

            if (orderRes.ok) {
                toast.success(orderData.message, {
                    position: "top-right",
                    autoClose: 3000,
                    hideProgressBar: true,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined,
                    theme: "colored",
                });
                fetchOwnerProductsAndIngredients();
            } else {
                toast.error(orderData.message, {
                    position: "top-right",
                    autoClose: 3000,
                    hideProgressBar: true,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined,
                    theme: "colored",
                });
            }
        } catch (error) {
            console.log(error);
        }
    }

    return (
        <div className='product-card' id={product.type}>
            <div className='product-card-top'>
                <div className='product-card-left'>
                    <img src={`/images/products/${productType.value}.png`} alt={`${productType.value}.png`} />
                </div>
                <div className='product-card-right'>
                    <h2>{productType.value}</h2>
                    <h3>Avaiable: {product.amount} pcs</h3>
                    <div className='product-card-input-container'>
                        <input type='number' step='0' aria-label='Quantity' placeholder='0' value={amount} onChange={e => setAmount(e.target.value)} />
                        <label className='product-card-unit-label'>pcs</label>
                    </div>
                </div>
            </div>
            <div className='product-card-bottom'>
                <button className='product-order-button' onClick={handleOrder}>Order</button>
            </div>
        </div>
    );
}