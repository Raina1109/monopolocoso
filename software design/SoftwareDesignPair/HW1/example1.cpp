#include <iostream>
#include <string>
#include <vector>

using namespace std;
 
class Unit
{
	public:
	Unit(const string& name) : name(name) {}
	virtual void talk() = 0;

	private:
	const string name;
};
 
class Soldier : public Unit
{
	public:
	Soldier(const string& name) : Unit(name) {}
	void talk() { cout << "Aye Aye Sir!\n"; }
};
 
class Peon : public Unit
{
	public:
	Peon(const string& name) : Unit(name) {}
	void talk() { cout << "Work Work Work!\n"; }
};
void Talk(Unit* unit)
{
    unit->talk();
}
int main()
{
        Peon* jack = new Peon("jack");
        Soldier* bill = new Soldier("bill");

        Talk (jack);
        Talk (bill);

	return 0;
}

