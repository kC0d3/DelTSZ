import { useState } from "react";
import { toast } from 'react-toastify';
import LoginError from "./LoginError";

export default function CreateProduct({ productTypes, loggedUser, setShowLogin }) {

    const [product, setProduct] = useState({ type: '', amount: '' });

    const handleCreate = async e => {
        e.preventDefault();

        try {
            const creatRes = await fetch('/api/products', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    type: Number(product.type),
                    amount: Number(product.amount)
                })
            });

            const createData = await creatRes.json();

            if (creatRes.ok) {
                toast.success(createData.message, {
                    position: "top-right",
                    autoClose: 3000,
                    hideProgressBar: true,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined,
                    theme: "colored",
                });
                setTimeout(() => {
                    setProduct({ type: '', amount: '' })
                }, 1000);
            } else {
                toast.error(createData.message, {
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
        loggedUser ?
            <form className='create-product' onSubmit={handleCreate}>
                <h1>Create product</h1>
                <select className='products' value={product.type} onChange={e => setProduct({ ...product, type: e.target.value })}>
                    <option value='' disabled hidden>Select product</option>
                    {productTypes.map((ingredient, index) => (<option key={index} value={ingredient.index}>{ingredient.value}</option>))}
                </select>
                <div className='input-container'>
                    <input type='number' aria-label='Quantity' placeholder='0' value={product.amount} onChange={e => setProduct({ ...product, amount: e.target.value })} />
                    <label className='unit-label'>pcs</label>
                </div>
                <button type='submit' className='create-button'>Create</button>
            </form>
            :
            <LoginError {...{ setShowLogin }} />
    );
}