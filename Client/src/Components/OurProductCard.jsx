export default function OurProductCard({ product, description }) {
    return (
        <div className='our-product-card' id={product.index}>
            <div className='our-product-card-details'>
                <div className='our-product-card-front'>
                    <img src={`/images/products/${product.value}.png`} alt={`${product.value}.png`} />
                    <h2>{description.name}</h2>
                </div>
                <div className='our-product-card-back'>
                    <img src={`/images/products/${product.value}.png`} alt={`${product.value}.png`} />
                    <div className='our-product-details'>
                        <h3>{description.name}</h3>
                        <p>{description.description}</p>
                    </div>
                </div>
            </div>
        </div>
    );
}