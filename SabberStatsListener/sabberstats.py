from typing import *
import re
import pandas as pd


# region Playables
class Playable:
    __name__ = 'Playable'

    def __init__(self, cost: int, name: str):
        self.cost: int = cost
        self.name: str = name


class Spell:
    __name__ = 'Spell'

    def __init__(self, cost: int, name: str):
        super().__init__(cost, name)


class Minion(Playable):
    __name__ = 'Minion'

    def __init__(self, cost: int, name: str, atk: int, hp: int):
        super().__init__(cost, name)
        self.atk: int = atk
        self.hp: int = hp


class Weapon(Playable):
    __name__ = 'Weapon'

    def __init__(self, cost: int, name: str, atk: int, dur: int):
        super().__init__(cost, name)
        self.atk: int = atk
        self.dur: int = dur


class HeroCard(Playable):
    __name__ = 'HeroCard'

    def __init__(self, cost: int, name: str):
        super().__init__(cost, name)


# endregion

class Hero:
    __name__ = 'Hero'

    def __init__(self, name: str, mana_remaining: int, total_mana: int, atk: int,
                 armor: int, hp: int, weapon: Optional[Weapon], spell_dmg: int):
        self.name: str = name
        self.mana_remaining: int = mana_remaining
        self.total_mana: int = total_mana
        self.atk: int = atk
        self.armor: int = armor
        self.hp: int = hp
        self.weapon: Optional[Weapon] = weapon
        self.spell_dmg: int = spell_dmg
        self.hand: List[Playable] = []
        self.board: List[Minion] = []


class SabberStats:
    __name__ = 'SabberStats'

    def __init__(self, turn: str, state: List[str]):
        """
        :param state:
        """
        self.turn: int = int(turn.strip().strip('|')[-1])

        self.player1: Optional[Hero] = None
        self.player2: Optional[Hero] = None
        self.players: List[Hero] = [self.player1, self.player2]

        self.current_player: Optional[Hero] = self.player1
        self.current_opp: Optional[Hero] = self.player2

        self.parse(state)
        # self.TotalMana: int = 0
        # self.ManaRemaining: int = 0
        # self.atk: int = 0
        # self.armor: int = 0
        # self.opp_armor: int = 0
        # self.hp: int = 30
        # self.opp_hp: int = 30
        # self.weapon: Optional[Weapon] = None
        # self.opp_weapon: Optional[Weapon] = None
        # self.spell_dmg: int = 0
        # self.hand: List[Playable] = []
        # self.board: List[Minion] = []
        # self.opp_board: List[Minion] = []
        # self.secrets: List[Spell] = []

    def parse(self, state: List[str]):
        """
        :param state:
        :return:
        """
        self.player2 = self.parse_hero(state[1])
        self.player2.hand = self.parse_zone(state[3])
        self.player2.board = self.parse_zone(state[5])

        self.player1.board = self.parse_zone(state[10])
        self.player1.hand = self.parse_zone(state[12])
        self.player1 = self.parse_hero(state[14])

    def parse_hero(self, line: str):
        """
        :param line:
        :return:
        """
        hero = line.strip().split('|')
        name = hero[0]
        mana = hero[1].split(':')[1].split('/')
        mana_remaining = int(mana[0])
        total_mana = int(mana[1])
        atk = int(hero[2].strip('ATK:'))
        armor = int(hero[3].strip('AR'))
        hp = int(hero[4].strip('HP:'))
        weapon = self.parse_weapon(hero[5])
        spell_dmg = int(hero[6].strip("SP+:"))
        return Hero(name, mana_remaining, total_mana, atk, armor, hp, weapon, spell_dmg)

    @staticmethod
    def parse_weapon(wpn_str: str):
        if wpn_str == "NO WEAPON":
            return None

        wpn_str = wpn_str.strip("WEAPON:[").split("--[")
        cost_name = wpn_str[0].split("]")
        cost = int(cost_name[0])
        name = cost_name[1]
        stats = wpn_str[1].strip("]").split('/')
        atk = int(stats[0])
        dur = int(stats[1])
        return Weapon(cost, name, atk, dur)

    @staticmethod
    def parse_zone(zone_str: str):
        zone_str = zone_str.split('|')
        zone = []
        for p in zone_str:
            p = re.sub(r"(\w)([A-Z])", r"\1 \2", p)
            type = re.search("\\[([MSW]*?)\\]", p).group(0)
            cost = int(re.search("\\[(\\d*?)\\]", p).group(0))
            name = re.search("[A-Za-z']+\\s\\w+|[A-Za-z]+$", p).string

            if type == ("M" or "W"):
                stats = re.search("\\d/\\d", p).string.split("/")
                atk = int(stats[0])
                d = int(stats[1])
                if type == "M":
                    zone.append(Minion(cost, name, atk, d))
                else:
                    zone.append(Weapon(cost, name, atk, d))
                    
            else:
                zone.append(Spell(cost, name))
