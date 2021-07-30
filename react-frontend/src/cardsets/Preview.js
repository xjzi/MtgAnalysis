import React, { useEffect, useState } from 'react';

import Card from '../common/Card';
import DataHandler from '../common/DataHandler';
import GetImage from '../common/GetImage';
import { Link } from "react-router-dom";
import styles from './Previews.module.css';

export default function Preview({ id, top }){
  const [error, setError] = useState(false);
  const [thumbnail, setThumbnail] = useState(null);

  useEffect(() => {
    async function fetchThumbnail(){
      const image = await GetImage(top[0], 'art_crop');
      if(image){
        setThumbnail(image);
      } else {
        console.error('API response was not formatted as expected.');
        setError(true)
      }
    }
    fetchThumbnail();
  }, [top]);

  const render = () => {
    const cards = top.map((card) => <Card key = {card} card={card} />);
    const style = { backgroundImage: `url(${thumbnail})` };
    return (
      <div className={styles['preview']}>
        <Link to={`/themes/${id}`}>
          <div className={styles['thumbnail-box']} style={style}/>
        </Link>
        <div className={styles['preview-card-box']}>
          {cards}
        </div>
      </div>
    );
  }

  return <DataHandler error={error} data={thumbnail} render={render}/>;
}