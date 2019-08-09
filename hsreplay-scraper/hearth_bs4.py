#!/usr/bin/env python

# import os
import cPickle as pickle
import time
from contextlib import closing

from bs4 import BeautifulSoup
from selenium.webdriver import Chrome
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.support.ui import WebDriverWait
from selenium.common.exceptions import *

# Constants
DECKS_PER_PAGE = 18.0

# UBLOCK_PATH = os.getcwd() + "\\1.17.0_0"
# UBLOCK = Options()
# UBLOCK.add_argument('load-extension=' + UBLOCK_PATH)
# UBLOCK.add_argument('--ignore-certificate-errors')
# UBLOCK.add_argument('--ignore-ssl-errors')

COOKIES = Options()
COOKIES.add_argument('user-data-dir=C:/Users/hgore/AppData/Local/Google/Chrome/User Data/')

BROWSER_PATH = r'C:\Program Files (x86)\Google\Chrome\Application\chromedriver.exe'
FIRST_URL = "https://hsreplay.net/decks/?modal=collection#timeRange=CURRENT_SEASON"
PAGE_URL = "https://hsreplay.net/decks/?modal=collection#timeRange=CURRENT_SEASON&page="


class Deck:

    """
    An object representing a single Hearthstone deck pulled from HearthPwn.
    """

    def __init__(self, archetype, win_rate, num_games, card_list):
        # returns (links, classes, types, ratings, dusts, epochs)
        """
        Initialize a HearthPwn Deck object.

        Parameters:

        - 'deck_id'   - the HearthPwn ID number of the deck (as seen in the URL)
        - 'hero_class'     - the Hearthstone class of the deck
        - 'type'     - the deck type (midrange, tempo, control, etc)
        - 'win_rate'   - the HearthPwn deck win_rate
        - 'dust'  - amount of dust needed to craft entire deck
        - 'card_list' - a list of Card objects
        """
        self.archetype = str(archetype)
        self.win_rate = str(win_rate)
        self.num_games = int(num_games)
        # self.dust = int(dust)
        if card_list is not None:
            self.card_list = card_list
        else:
            self.card_list = []

    def __repr__(self):
        output = 'Archetype: ' + str(self.archetype) + '\n' + 'Win Rate: ' + str(self.win_rate) + '\n'
        for card in self.card_list:
            output += card + '\n'
        return output

    def __contains__(self, item):
        return item in self.card_list

    def __eq__(self, other):
        """
        Overloaded equality operator
        :param other:
        :return:
        """
        for i in range(len(self.card_list)):
            if self.card_list[i] != other.card_list[i]:
                return False
        return True


class DeckScraper:

    """
    The main webcrawler class that retrieves the decks from hsreplay.net
    """

    def __init__(self):
        """
        Initializes a webcralwer object
        """
        self.decks = {
            'DRUID': [], 'HUNTER': [], 'MAGE': [],
            'PALADIN': [], 'PRIEST': [], 'ROGUE': [],
            'SHAMAN': [], 'WARLOCK': [], 'WARRIOR': []
        }

    def build_decks(self):
        """
        Builds decks from hsreplay.net
        :return: a dictionary structured as {hero_class : [list of decks]}
        """
        with closing(Chrome(BROWSER_PATH, options=COOKIES)) as browser:
            browser.get(FIRST_URL)
            WebDriverWait(browser, timeout=10).until(
                lambda x: x.find_element_by_class_name('deck-list')
            )

            page = browser.page_source
            hsreplay = BeautifulSoup(page, "lxml")
            page_count = int(hsreplay.find("ul", class_="pagination").find_all(class_="visible-lg-inline")[-1].string)

            deck_counter = 1
            num_decks = page_count * DECKS_PER_PAGE
            curr_page = 2

            for i in xrange(page_count - 1):
                if i == 3:
                    with open("decks.txt", "r+") as f:
                        pickle.dump(self.decks, f)
                        f.close()

                url_to_use = ""
                if i > 0:
                    url_to_use = PAGE_URL + str(curr_page)
                    curr_page += 1

                    browser.get(url_to_use)
                    WebDriverWait(browser, timeout=10).until(
                        lambda x: x.find_element_by_class_name('deck-list')
                    )
                    page = browser.page_source
                    hsreplay = BeautifulSoup(page, "lxml")

                deck_list = hsreplay.find("section", class_="deck-list")
                deck_list = deck_list.find_all("ul", recursive=False)[0].contents[1:]

                for item in deck_list:
                    print "Adding deck ", deck_counter, " of ", num_decks
                    time.sleep(0.05)
                    deck_counter += 1

                    deck_container = item.contents[0]
                    deck_class = deck_container.contents[0].attrs['data-card-class']
                    deck_details = deck_container.contents[0].contents
                    deck_archetype = str(deck_details[0].contents[0].contents[0])
                    deck_win_rate = str(deck_details[1].string)
                    deck_num_games = str(deck_details[2].string)
                    if "," in deck_num_games:
                        deck_num_games = deck_num_games.replace(",", "")
                    deck_num_games = int(deck_num_games)
                    # deck_dust = int(deck_details[0].contents[1].contents[0].string)
                    card_list = deck_details[5].contents[0].contents

                    cards = []
                    append = cards.append

                    for card_container in card_list:
                        cards_info = card_container.find_all("div")[1].attrs['aria-label'].split(" " + u'\xd7')
                        if u'\u2605' in cards_info[0]:
                            card_name = cards_info[0].encode('ascii', 'ignore')[:-1]
                        else:
                            card_name = str(cards_info[0])
                        try:
                            card_count = str(cards_info[1])
                        except IndexError:
                            card_count = 1
                        if card_count == '2':
                            append(card_name)
                        append(card_name)

                    if len(self.decks[deck_class]) == 0:
                        self.decks[deck_class] = [Deck(deck_archetype, deck_win_rate, deck_num_games, cards)]
                    else:
                        self.decks[deck_class].append(Deck(deck_archetype, deck_win_rate, deck_num_games, cards))

        with open("decks.txt", "r+") as f:
            pickle.dump(self.decks, f)
        f.close()


def main():
    scraper = DeckScraper()
    scraper.build_decks()


if __name__ == "__main__":
    # Execute only if run as a script
    main()


# def populate_deck_db(decks, cursor):
#     """
#     (Re)populates deck information in the SQLite database.
#
#     Parameters:
#
#     - 'decks' - a list of Deck objects
#     - 'cursor' - a SQLite3 cursor object
#     """
#     cursor.execute('DROP TABLE IF EXISTS decks')
#     cursor.execute('DROP TABLE IF EXISTS deck_lists')
#     cursor.execute('''CREATE TABLE IF NOT EXISTS decks
#              (deck_id integer primary key, class text, type text,
#              win_rate integer, dust integer, updated integer)''')
#     cursor.execute('''CREATE TABLE IF NOT EXISTS deck_lists
#              (deck_id integer, card_name text, amount integer,
#               PRIMARY KEY (deck_id, card_name))''')
#     for deck in decks:
#         cursor.execute('''INSERT INTO decks (class, type, win_rate, dust, updated)
#                         VALUES ( ?, ?, ?, ?, ?)''',
#                        (deck.hero_class, deck.type, deck.win_rate,
#                         deck.dust, deck.updated))
#         last_id = cursor.lastrowid
#         for card in deck.card_list:
#             cursor.execute('INSERT INTO deck_lists VALUES (?, ?, ?)',
#                            (last_id, card.card_name, card.amount))
#     return
#
#
# def get_cards(mashape_key):
#     """
#     Gets a list of all current Hearthstone cards from omgvamp's mashape
#     Hearthstone API, and returns them as a json object.
#
#     Parameters:
#
#     - 'mashape_key' - a string containing a Mashape API key
#     """
#     if len(mashape_key) <= 0:
#         print('Mashape API key does not exist in config.ini')
#         sys.exit(-1)
#     url = "https://omgvamp-hearthstone-v1.p.mashape.com/cards?collectible=1"
#     headers = {"X-Mashape-Key": mashape_key}
#     response = requests.get(url, headers=headers)
#     try:
#         cards = json.loads(response.text)
#     except json.decoder.JSONDecodeError:
#         print("Unable to decode (possibly empty) response.")
#         print("Response: " + response.text)
#         sys.exit(-1)
#     return cards
#
#
# def populate_card_db(cards, cursor):
#     """
#     Populates card information in the SQLite database.
#
#     Parameters:
#
#     - 'cards' - a JSON object containing a card collection, obtained from the
#               Mashape API
#     - 'cursor' - a SQLite3 cursor object
#     """
#     cursor.execute('DROP TABLE IF EXISTS cards')
#     cursor.execute('''CREATE TABLE IF NOT EXISTS cards
#                       (card_name text, cardset text,
#                        hero_class text, rarity text,
#                        PRIMARY KEY (card_name))''')
#     # Removing invalid sets from our results. For the most part, these sets are
#     # empty lists as we filter out non-collectible cards. The Mashape API
#     # includles cardsets without collectible cards, such as 'System',
#     # 'Credits', and 'Debug'. We also explicitly remove the 'hero_class Skins' set as
#     # they are considered "collectible cards" by HearthStone, but not for our
#     # purposes. We will filter out cards where "type": "hero_class" later for
#     # similar reasons.
#     valid_cardsets = {cardset: cards for cardset, cards in cards.items()
#                       if cards and cardset != 'hero_class Skins'}
#     for cardset in valid_cardsets:
#         for card in cards[cardset]:
#             if card['type'] != 'hero_class':
#                 cursor.execute('INSERT INTO cards VALUES (?, ?, ?, ?)',
#                                (card['name'], card['cardSet'],
#                                 card.get('playerClass', 'Neutral'),
#                                 card['rarity']))
#     return
#
#
# def get_collection(auth_session):
#     """
#     Gets a list of all cards in your HearthPwn collection,
#     and returns them as a json object.
#
#     Parameters:
#
#     - 'auth_session' - a string containing a HearthPwn Auth.Session cookie
#     """
#     if len(auth_session) <= 0:
#         print('Auth Session does not exist in config.ini')
#         sys.exit(-1)
#     url = "https://www.hearthpwn.com/ajax/collection"
#     cookies = dict({'Auth.Session': auth_session})
#     response = requests.get(url, cookies=cookies)
#     try:
#         collection = json.loads(response.text)
#     except json.decoder.JSONDecodeError:
#         print("Unable to decode (possibly empty) response.")
#         print("Response: " + response.text)
#         sys.exit(-1)
#     return collection
#
#
# def populate_collection_db(collection, cursor):
#     """
#     Populates collection information in the SQLite database.
#
#     Parameters:
#
#     - 'collection' - a JSON object containing a card collection, obtained from
#                    https://www.hearthpwn.com/ajax/collection
#     - 'cursor' - a SQLite3 cursor object
#     """
#     # TODO: Possibly skip all of this if the collection hasn't been updated
#     # since the last time this was ran.
#     # The collection JSON contains the last update time:
#     # Ex: { "updatedDate":"4/20/2017 2:39:54 PM"
#     cursor.execute('DROP TABLE IF EXISTS collection')
#     cursor.execute('''CREATE TABLE IF NOT EXISTS collection
#                       (card_name text, amount integer,
#                        PRIMARY KEY (card_name))''')
#
#     total = len(collection['cards'])
#     for counter, card in enumerate(collection['cards']):
#         print("Adding card " + str(counter+1) + " of " + str(total) +
#               " to collection.")
#         card_name = get_cardname(card['externalID'], cursor)
#         # HearthPwn can return 3/4 if you have normal + gold copies of a card.
#         # We just care how many "usable" copies you have, regardless of rarity.
#         amount = min(card['count'], 2)
#         cursor.execute('INSERT INTO collection VALUES (?, ?)',
#                        (card_name, amount))
#     return
#
#
# def get_cardname(card_id, cursor):
#     """
#     Given a HearthPwn card ID, retrieve the card_name for that ID.
#     First attempts to find the card_name in the local database,
#     and if it's not found, looks up the ID on HearthPwn and stores
#     that name in the local DB.
#
#     Parameters:
#
#     - 'card_id' - the integer ID of the card to find the name of
#     - 'cursor' - a SQLite3 cursor object
#     """
#     url = "https://www.hearthpwn.com/cards/" + str(card_id)
#     css = "#content > section > div > header.h2.no-sub.with-nav > h2"
#
#     cursor.execute('''CREATE TABLE IF NOT EXISTS card_ids
#                       (card_name text, cardid integer,
#                        PRIMARY KEY (cardid))''')
#
#     cursor.execute('SELECT card_name FROM card_ids WHERE cardid IS ?',
#                    (card_id,))
#     card_name = cursor.fetchone()
#
#     if card_name:
#         # We can't do this before without try/catch -- if there are no results,
#         # the query returns none, and trying to get the [0]th element of None
#         # results in a TypeError.
#         card_name = card_name[0]
#     else:
#         print('card_name for HearthPwn card ID ' + str(card_id) +
#               ' not found in local DB.')
#         # Card ID <-> Card Name mapping wasn't found in the local DB
#         html_element = get_html_element_from_url(url)
#         # cssselect always returns an array, but in our case the result
#         # should just be one element.
#         card_name = html_element.cssselect(css)[0].text.strip()
#         cursor.execute('INSERT INTO card_ids VALUES (?, ?)',
#                        (card_name, card_id))
#     return card_name
#
#
# def get_db_deck_updated(cursor, deck_id):
#     """
#     Returns the timestamp of the specified deck
#
#     Parameters:
#
#     - 'cursor' - a SQLite3 cursor object
#     - 'deck_id' - a HearthPwn deck ID
#     """
#     cursor.execute('SELECT updated FROM decks WHERE deck_id IS ?', (deck_id,))
#     return cursor.fetchone()[0]
#
#
# def get_db_card_percentages(cursor):
#     """
#     For all cards, return: (card_name, total decks using the card, percentage
#     of decks using the card, and average number of the card in a deck) from
#     the database.
#
#     Parameters:
#
#     - 'cursor' - a SQLite3 cursor object
#     """
#     sql = '''
#             select cards.card_name,
#                     cards.hero_class,
#                     case
#                         when deck_lists.card_name is null then 0
#                         else count(*)
#                     end as [total],
#                     avg(coalesce(deck_lists.amount, 0)) as [per deck],
#                     case
#                         when deck_lists.card_name is null then 0.0
#                         else count(*) /
#                         (select cast(count(*) as double) from decks) * 100.0
#                     end as [percent],
#                     coalesce(collection.amount, 0) as collected
#             from cards
#             left join deck_lists
#             on cards.card_name = deck_lists.card_name
#             left join collection
#             on cards.card_name = collection.card_name
#             where cards.cardset in ('Classic',
#                                     'Whispers of the Old Gods',
#                                     'Mean Streets of Gadgetzan',
#                                     'Journey to Un''Goro')
#             group by cards.card_name
#             order by Total desc
#             '''
#     results = cursor.execute(sql)
#     return results
#
#
# if __name__ == "__main__":
#     # Execute only if run as a script
#     main()
