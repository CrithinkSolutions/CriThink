import React, { Component } from "react";
import { Link } from "react-router-dom";
import { WaveDown } from "./../Layout";
import { Card, Image, Icon, Popup } from "semantic-ui-react";
import Book from "./../../svg/bookreader.svg";
import Check from "./../../svg/check.svg";
import Graduation from "./../../svg/graduation.svg";
import LogoRound from "./../../svg/logoroundw.svg";

export class SelectionArea extends Component {
  render() {
    return (
      <div>
        <WaveDown>
          <Card.Group itemsPerRow={4} className="menucard">
            <Card>
              <Card.Content>
                <p className="regular menutitle">Check Your News</p>
                <Image src={Book} />
                <p className='light p-4'>Check & critically evaluate your news with the HEAD.</p>
              </Card.Content>
              <Link to="/source-selection" className='txtRight p-3'>
                <Popup
                  content="Check your news"
                  trigger={
                    <span className="whitelink bold">
                      CHECK <Icon name="long arrow alternate right" />
                    </span>
                  }
                ></Popup>
              </Link>
            </Card>

            <Card>
              <Card.Content>
                <p className="regular menutitle">Debunked Fake News</p>
                <Image src={Check} style={{ height: "59px" }} />
                <p className='light p-4'>Read the latest debunked fake news.</p>
              </Card.Content>
              <div className='txtRight p-3'>
                <Popup
                      content="Section under construction..."
                      trigger={
                        <span className="nolink bold">
                          DEBUNK <Icon name="long arrow alternate right" />
                        </span>
                      }
                ></Popup>
              </div>
            </Card>

            <Card>
              <Card.Content>
                <p className="regular menutitle">Spot Fake News</p>
                <Image src={Graduation} />
                <p className='light p-4'>Become a fake news expert and discover how to spot it.</p>
              </Card.Content>
              <div className='txtRight p-3'>
                <Popup
                      content="Section under construction..."
                      trigger={
                        <span className="nolink bold">
                          LEARN <Icon name="long arrow alternate right" />
                        </span>
                      }
                ></Popup>
              </div>
            </Card>

            <Card>
              <Card.Content>
                <p className="regular menutitle">Quiz & Games</p>
                <Image src={LogoRound} />
                <p className='light p-4'>Enjoy our games and quizzes.</p>
              </Card.Content>
              <div className='txtRight p-3'>
                <Popup
                      content="Section under construction..."
                      trigger={
                        <span className="nolink bold">
                          PLAY <Icon name="long arrow alternate right" />
                        </span>
                      }
                ></Popup>
              </div>
            </Card>
          </Card.Group>
        </WaveDown>
      </div>
    );
  }
}
