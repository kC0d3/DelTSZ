export default function ProductCard({ product, description }) {
    return (
        <div className='product-card' id={product.index}>
            <div className='product-card-details'>
                <div className='product-card-front'>
                    <img src={`/images/products/${product.value}.png`} alt={`${product.value}.png`} />
                    <h2>{description.name}</h2>
                </div>
                <div className='product-card-back'>
                    <img src={`/images/products/${product.value}.png`} alt={`${product.value}.png`} />
                    <div className='product-details'>
                        <h3>{description.name}</h3>
                        <p>{description.description}</p>
                    </div>
                </div>
            </div>
        </div>
    );
}