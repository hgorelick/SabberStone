#!/usr/bin/env python

import cPickle as pickle
from collections import Counter
from hearth_bs4 import Deck
import sqlite3


class DeckWriter:

    def __init__(self, decks):
        """
        Initializes a CsDeckWriter object
        :param decks:
        """
        self.decks = decks

        self.classes = [
            'DRUID',
            'HUNTER',
            'MAGE',
            'PALADIN',
            'PRIEST',
            'ROGUE',
            'SHAMAN',
            'WARLOCK',
            'WARRIOR'
        ]

        self.druid_deck_names = []
        self.hunter_deck_names = []
        self.mage_deck_names = []
        self.paladin_deck_names = []
        self.priest_deck_names = []
        self.rogue_deck_names = []
        self.shaman_deck_names = []
        self.warlock_deck_names = []
        self.warrior_deck_names = []

        self.class_names = [
            self.druid_deck_names,
            self.hunter_deck_names,
            self.mage_deck_names,
            self.paladin_deck_names,
            self.priest_deck_names,
            self.rogue_deck_names,
            self.shaman_deck_names,
            self.warlock_deck_names,
            self.warrior_deck_names
        ]

        self.three_tabs = "\t\t\t"
        self.four_tabs = self.three_tabs + "\t"
        self.five_tabs = self.four_tabs + "\t"
        self.six_tabs = self.five_tabs + "\t"
        self.seven_tabs = self.six_tabs + "\t"

        self.header = "using System.Collections.Generic;" + "\n" + "using SabberStoneCore.Model;" + "\n\n" \
            + "namespace SabberStoneCoreAi.Meta" + "\n{\n" + "\tpublic class "

        self.deck_part_one = "Decks" + "\n\t" + "{\n" + "\t\tpublic List<Deck> decks;" + "\n\n" + "\t\tpublic "
        self.deck_part_two = "Decks()\n" + "\t\t{" + "\n" + self.three_tabs + "decks = new List<Deck> {" + "\n"

        self.new_deck = self.four_tabs + "new Deck(" + "\n" + self.six_tabs + "\""
        self.close_new_deck = "\",\n"
        self.new_list = self.six_tabs + "new List<Card>" + "\n" + self.six_tabs + "{\n"
        self.card_format = self.seven_tabs + "Cards.FromName(\""
        self.close_deck = self.six_tabs + "}\n" + self.six_tabs + ")"
        self.close_decks = "\n" + self.four_tabs + "};" + "\n\t\t" + "}"
        self.close_file = "\n\t}\n}"

    def write_decks_to_csharp(self):
        """
        Writes decks generated by hearth_bs4.py in SabberStone's c# format
        :return:
        """
        for hero_class in self.decks:
            print "WRITING", len(self.decks[hero_class]), hero_class, "DECKS..."
            with open(self.create_file_name(hero_class), 'w+') as f:
                f.write(self.header)
                f.write(self.create_class_name(hero_class))
                for deck in self.decks[hero_class]:
                    f.write(self.new_deck + deck.archetype + self.close_new_deck + self.six_tabs)
                    f.write(str(deck.num_games))
                    f.write(',\n' + self.new_list)
                    for card in deck.card_list:
                        f.write(self.write_card(card))
                    f.write(self.close_deck)
                    if deck == self.decks[hero_class][-1]:
                        f.write(self.close_decks)
                    else:
                        f.write(',\n')
                f.write(self.close_file)
            f.close()

    @staticmethod
    def create_file_name(hero_class):
        """
        Returns a filename based on the hero_class
        :param hero_class:
        :return:
        """
        return "C:\Users\hgore\SabberStone\core-extensions\SabberStoneCoreAi\src\Meta\\" + hero_class + "_decks.cs"

    def create_class_name(self, hero_class):
        """
        Creates the deck-class name (i.e. HunterDecks)
        :param hero_class:
        :return:
        """
        return hero_class + self.deck_part_one + hero_class + self.deck_part_two

    def process_decks(self):
        """
        Populates self.deck_names and self.deck_name_counts before the decks are written
        :return:
        """
        for hero_class in self.decks:
            for deck in self.decks[hero_class]:
                deck.archetype = self.process_deck_name(deck.archetype)
                if hero_class == 'DRUID':
                    self.druid_deck_names.append(deck.archetype)
                elif hero_class == 'HUNTER':
                    self.hunter_deck_names.append(deck.archetype)
                elif hero_class == 'MAGE':
                    self.mage_deck_names.append(deck.archetype)
                elif hero_class == 'PALADIN':
                    self.paladin_deck_names.append(deck.archetype)
                elif hero_class == 'PRIEST':
                    self.priest_deck_names.append(deck.archetype)
                elif hero_class == 'ROGUE':
                    self.rogue_deck_names.append(deck.archetype)
                elif hero_class == 'SHAMAN':
                    self.shaman_deck_names.append(deck.archetype)
                elif hero_class == 'WARLOCK':
                    self.warlock_deck_names.append(deck.archetype)
                elif hero_class == 'WARRIOR':
                    self.warrior_deck_names.append(deck.archetype)

        druid_counts = Counter(self.druid_deck_names)
        hunter_counts = Counter(self.hunter_deck_names)
        mage_counts = Counter(self.mage_deck_names)
        paladin_counts = Counter(self.paladin_deck_names)
        priest_counts = Counter(self.priest_deck_names)
        rogue_counts = Counter(self.rogue_deck_names)
        shaman_counts = Counter(self.shaman_deck_names)
        warlock_counts = Counter(self.warlock_deck_names)
        warrior_counts = Counter(self.warrior_deck_names)

        counts = [druid_counts, hunter_counts, mage_counts, paladin_counts, priest_counts, rogue_counts,
                  shaman_counts, warlock_counts, warrior_counts]

        i = 0
        for class_counts in counts:
            for name, count in class_counts.items():
                if count > 1:
                    for suffix in range(1, count + 1):
                        self.class_names[i][self.class_names[i].index(name)] = name + str(suffix)
            i += 1

        for i in range(len(self.decks)):
            for j in range(len(self.decks[self.classes[i]])):
                self.decks[self.classes[i]][j].archetype = self.class_names[i][j]

    def write_card(self, card):
        """
        Writes a card in the correct format for SabberStone
        :param card:
        :return:
        """
        return self.card_format + card + "\"),\n"

    @staticmethod
    def process_deck_name(deck_name):
        """
        Processes deck_names so that there are no double declarations
        :param deck_name:
        """
        if " " in deck_name:
            if "Mecha'thun" in deck_name:
                return deck_name.replace(" ", "_").replace("'", "")
            return deck_name.replace(" ", "_")
        return deck_name

    def write_deck_db(self):
        """
        Writes decks to an SQLite3 database
        """
        print("Connecting to SQLite3")
        conn = sqlite3.connect(r'C:\Users\hgore\SabberStone\SabberStoneCore\resources\Data\hearth_decks.db')
        cursor = conn.cursor()
        print("SQLite3 Connected")

        cursor.execute('DROP TABLE IF EXISTS decks')
        cursor.execute('DROP TABLE IF EXISTS deck_cards')
        cursor.execute('''CREATE TABLE IF NOT EXISTS decks
                     (hero_class text, archetype text, deck_name text, num_games integer)''')
        cursor.execute('''CREATE TABLE IF NOT EXISTS deck_cards
                     (deck_name, card_name text)''')

        for hero_class in self.decks:
            for deck in self.decks[hero_class]:
                cursor.execute('''INSERT INTO decks (hero_class, archetype, deck_name, num_games)
                                    VALUES (?, ?, ?, ?)''',
                               (hero_class, deck.archetype.split("_")[0], deck.archetype,
                                deck.num_games))
                # last_id = cursor.lastrowid
                for card in deck.card_list:
                    cursor.execute('INSERT INTO deck_cards VALUES (?, ?)',
                                   (deck.archetype, card))

        conn.commit()
        conn.close()
        print ('Complete!')


if __name__ == "__main__":
    print "WRITING DECKS FOR C#"
    deck_list = pickle.load(open('./decks.txt', 'r+'))
    deck_writer = DeckWriter(deck_list)
    deck_writer.process_decks()
    deck_writer.write_deck_db()
    deck_writer.write_decks_to_csharp()