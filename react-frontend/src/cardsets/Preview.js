import React, { useEffect, useState } from 'react';

import Card from '../common/Card';
import DataHandler from '../common/DataHandler';
import FetchJson from '../common/FetchJson';
import { Link } from "react-router-dom";
import styles from './Previews.module.css';

export default function Preview({ id, top }){
  const [error, setError] = useState(false);
  const [thumbnail, setThumbnail] = useState(null);

  useEffect(() => {
    async function fetchThumbnail(){
      const url = new URL('https://api.scryfall.com/cards/named');
      url.searchParams.append('exact', top[0]);
      const json = await FetchJson(url);
      const art_crop = json.image_uris?.art_crop ?? json.card_faces[0]?.image_uris.art_crop;
      if(art_crop){
        setThumbnail(art_crop);
      } else{
        console.error('API response was not formatted as expected.');
        setError(true);
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