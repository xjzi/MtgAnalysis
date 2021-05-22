import React from 'react';
import Card from './Card';

class Preview extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      url: null,
      cluster: props.cluster
    };
  }

  componentDidMount() {
    var url = new URL('https://api.scryfall.com/cards/named');
    url.searchParams.append('exact', this.state.cluster.highlights[0]);

    fetch(url)
      .then(res => res.json())
      .then(
        (result) => {
          this.setState({
            url: result.image_uris.art_crop
          });
        },
        (error) => {
          console.error(error);
        }
      );
  }

  render() {
    return (
      <div className="Preview">
        <img
          src={this.state.url}
          alt="thumbnail card for this deck"
          className="Thumbnail"
        >
        </img>
        {
          this.state.cluster.highlights.map((obj) => {
            return <div><Card card={obj}/></div>;
          })
        }
        <br></br>
      </div>
    );
  }
}

export default Preview;