export default function ProfileProductCard({ product, productTypes }) {
    const productType = productTypes.find(type => type.index === product.type);

    return (
        <div className='profile-product-card' id={product.id}>
            <div className='profile-product-card-top'>
                <div className='profile-product-card-left'>
                    <img src={`/images/products/${productType.value}.png`} alt={`${productType.value}.png`} />
                </div>
                <div className='profile-product-card-right'>
                    <div className='profile-product-card-rigth-top'>
                        <h2>{productType.value}</h2>
                        <h3><span>Amount</span></h3>
                        <h3>{product.amount} pcs</h3>
                    </div>
                    <div className='profile-product-card-rigth-bottom'>
                        <h3><span>Packed:</span></h3>
                        <h3>{product.packed.slice(0, 10)}</h3>
                    </div>
                </div>
            </div>
        </div>
    );
}