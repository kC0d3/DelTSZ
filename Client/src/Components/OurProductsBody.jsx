import ProductCard from "./ProductCard";

export default function OurProductsBody({ productTypes, productDescriptions }) {
    return (
        <div className='our-products-body'>
            {productTypes && productTypes.map((product, index) => (
                <ProductCard key={index}{...{ product }} description={productDescriptions[product.value]} />
            ))}
        </div>
    );
}