import React, { useEffect, useState } from 'react';

import DataHandler from '../common/DataHandler';
import GetImage from '../common/GetImage';
import styles from './CardInfo.module.css';

export default function CardInfo({ card }){
  const [error, setError] = useState(false);
  const [image, setImage] = useState(null);

  useEffect(() => {
    async function fetchImage(){
      const image = await GetImage(card, 'normal');
      if(image){
        setImage(image);
      } else {
        console.error('API response was not formatted as expected.');
        setError(true)
      }
    }
    fetchImage();
  }, [card]);

  const render = () => {
    const style = { backgroundImage: `url(${image})` };
    return <div className={styles['image']} style={style}/>;
  }

  return <DataHandler error={error} data={image} render={render}/>;
}