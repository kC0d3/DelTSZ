import { useEffect, useState } from 'react'

function App() {
  const [productTypes, setProductTypes] = useState([]);

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const response = await fetch('/api/products/types');
      const data = await response.json();
      console.log(data);
      setProductTypes(data);
    } catch (error) {
      console.log(error);
    }
  }

  return (
    <>
      {productTypes && productTypes.map((product, index) => {
        return <div key={index}>
          <div>{product.index}</div>
          <div>{product.value}</div>
        </div>
      })}
    </>
  )
}

export default App
