from hsreplay import document
import hearthstone
import hslog
import lxml
import dateutil

if __name__ == '__main__':
	xml = document.HSReplayDocument()
	xml = xml.from_xml_file(open(r"C:\Users\hgore\SabberStone\core-extensions\SabberStoneCoreAi\src\Meta\SabberStone.xml", "r+"))
	# xml = xml.from_log_file(open(r"C:\Program Files (x86)\Hearthstone\Logs\Power.log", "r+"))

	with open(r"C:\Users\hgore\SabberStone\core-extensions\SabberStoneCoreAi\src\Meta\SabberStone1.xml", "w+") as newf:
		newf.write(xml.to_xml(pretty=True))

	print(xml)
