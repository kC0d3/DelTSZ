import ProductCard from "./ProductCard";

export default function OurProductsBody({ productAndIngredientTypes, productDescriptions }) {
    return (
        <div className='our-products-body'>
            {productAndIngredientTypes.map((product, index) => (
                <ProductCard key={index} {...{ product }} description={productDescriptions[product.value]} />
            ))}
        </div>
    );
}